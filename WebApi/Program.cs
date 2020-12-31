using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            await MigrateDatabaseAsync(scope.ServiceProvider);

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        private static async Task MigrateDatabaseAsync(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                if (context.Database.IsInMemory())
                {
                    logger.LogWarning("Using in-memory database, no data will be stored.");
                    return;
                }

                await context.Database.MigrateAsync();
                logger.LogInformation("Migration(s) applied");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                throw;
            }
        }
    }
}
