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
            //app.MapPost(
            //    "/register_dto",
            //    async (
            //        RegisterDto model,
            //        UserManager<Gebruiker> userManager,
            //        CarAndAllContext dbContext
            //    ) =>
            //    {
            //        // Maak de gebruiker aan als een Klant
            //        var klant = new Klant
            //        {
            //            UserName = model.Email,
            //            Naam = model.Naam,
            //            Adres = model.Adres,
            //            Email = model.Email,
            //            Type = model.Type, // Het type klant (Particulier of Zakelijk)
            //        };

            //        // Als het een zakelijke klant is, koppel het bedrijf
            //        if (model.Type == KlantType.ZakelijkeBeheerder && model.BedrijfId.HasValue)
            //        {
            //            var bedrijf = await dbContext.Bedrijven.FindAsync(model.BedrijfId.Value);
            //            if (bedrijf == null)
            //            {
            //                return Results.BadRequest("Bedrijf niet gevonden.");
            //            }

            //            klant.Bedrijf = bedrijf;
            //        }

            //        // Probeer de gebruiker aan te maken in Identity
            //        var result = await userManager.CreateAsync(klant, model.Password);

            //        if (result.Succeeded)
            //        {
            //            // Registratie is succesvol
            //            return Results.Ok("Klant succesvol geregistreerd.");
            //        }

            //        // Fout bij registratie
            //        return Results.BadRequest(result.Errors);
            //    }
            //);

            app.Run();
        }
    }
}
