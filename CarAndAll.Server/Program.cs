using CarAndAll.Server.Data;
using CarAndAll.Server.DTOs;
using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CarAndAll.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create and configure the WebApplication
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CarAndAllContext>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("https://localhost:60281")
                       .AllowCredentials()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
            });

            

            // Register other services, e.g., controllers, etc.
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            //builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(
            //    c =>
            //{
            //    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            //}
            );

            builder
                .Services.AddIdentityApiEndpoints<Gebruiker>()
                .AddEntityFrameworkStores<CarAndAllContext>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapIdentityApi<Gebruiker>();
            //app.MapCustomIdentityEndpoints();
            app.MapPost(
                "/register_dto",
                async (RegisterDto model, UserManager<Gebruiker> userManager) =>
                {
                    var user = new Gebruiker
                    {
                        UserName = model.Email,
                        Naam = model.Naam,
                        Adres = model.Adres,
                        Email = model.Email,
                    };
                    Console.WriteLine("model: {}");
                    var result = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return Results.Ok("User registered successfully");
                    }

                    return Results.BadRequest(result.Errors);
                }
            );
            app.Run();
        }
    }
}
