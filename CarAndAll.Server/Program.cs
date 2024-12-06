using CarAndAll.Server.Data;
using CarAndAll.Server.DTOs;
using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarAndAll.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create and configure the WebApplication
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<CarAndAllContext>();

            // Register other services, e.g., controllers, etc.
            builder.Services.AddAuthorization(
                );
            builder.Services.AddControllers();
            //builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            builder.Services.AddIdentityApiEndpoints<Gebruiker>()
                .AddEntityFrameworkStores<CarAndAllContext>();
            

            var app = builder.Build();


            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapIdentityApi<Gebruiker>();
            //app.MapCustomIdentityEndpoints();
            app.MapPost("/register_dto", async (RegisterDto model, UserManager<Gebruiker> userManager) =>
            {
                var user = new Gebruiker
                {
                    UserName = model.Email,
                    Naam = model.Naam,
                    Adres = model.Adres,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Results.Ok("User registered successfully");
                }

                return Results.BadRequest(result.Errors);
            });
            app.Run();
        }
    }
}
