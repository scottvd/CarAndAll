using System.Security.Claims;
using System.IO;
using CarAndAll.Server.Data;
using CarAndAll.Server.DTOs;
using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace CarAndAll.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
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

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Huurders", policy =>
                    policy.Requirements.Add(new RolRequirement(new[] { "Particulier", "Zakelijk", "Wagenparkbeheerder" })));
                options.AddPolicy("Wagenparkbeheerder", policy =>
                    policy.Requirements.Add(new RolRequirement(new[] { "Wagenparkbeheerder" })));
                options.AddPolicy("Medewerkers", policy =>
                    policy.Requirements.Add(new RolRequirement(new[] { "FrontofficeMedewerker", "BackofficeMedewerker" })));
                options.AddPolicy("BackofficeMedewerker", policy =>
                    policy.Requirements.Add(new RolRequirement(new[] { "BackofficeMedewerker" })));
            });

            builder.Services.AddSingleton<IAuthorizationHandler, JwtAuthorizationHandler>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddIdentity<Gebruiker, IdentityRole>()
                .AddEntityFrameworkStores<CarAndAllContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-Token";
            });

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
            using(var scope = app.Services.CreateScope()) {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<CarAndAllContext>();
                context.Database.Migrate();
                var userManager = services.GetRequiredService<UserManager<Gebruiker>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                
                await DbSeeding.SeedDatabase(context, userManager, roleManager);
            }

            app.Run();
        }
    }
}
