using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YourNamespace;

namespace CarAndAll.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create and configure the WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Register the DbContext with the DI container
            builder.Services.AddDbContext<CarAndAllContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register other services, e.g., controllers, etc.
            builder.Services.AddControllers();

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
