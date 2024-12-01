using CarAndAll.Server.Data;
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

            // Register other services, e.g., controllers, etc.
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.UseRouting();

            // Map controllers to routes
            app.MapControllers();   

            // Run the application
            app.Run();
        }
    }
}
