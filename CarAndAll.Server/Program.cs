using System.Security.Claims;
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

            var connectionString = builder.Configuration.GetConnectionString("CarAndAllDbConnection") ?? throw new InvalidOperationException();
            builder.Services.AddDbContext<CarAndAllContext>(options =>
                options.UseSqlite(connectionString)
            );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                            .WithOrigins("https://localhost:60281")
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

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
            app.MapPost("/logout", async (SignInManager<Gebruiker> signInManager) => {
                await signInManager.SignOutAsync();
                return Results.Ok();
            }).RequireAuthorization();

            app.MapPost("/pingauth", (ClaimsPrincipal gebruiker) => {
                var email = gebruiker.FindFirstValue(ClaimTypes.Email);
                return Results.Json(new { Email = email}); ;
            }).RequireAuthorization();

            app.Run();
        }
    }
}
