using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            await MigrateDatabaseAsync(scope.ServiceProvider);

            await SeedIdentityAsync(scope.ServiceProvider);
            await SeedDatabaseAsync(scope.ServiceProvider);

            try
            {
                Log.Information("Starting host...");

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

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
                logger.LogError(ex, "An error occurred while migrating or seeding the database");
                throw;
            }
        }

        private static async Task SeedDatabaseAsync(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                await ApplicationDbContextSeed.SeedSampleDataAsync(context);
                logger.LogInformation("Default database content created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database content");
                throw;
            }
        }

        private static async Task SeedIdentityAsync(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                // Create the roles
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await IdentitySeed.SeedDefaultRolesAsync(roleManager);
                logger.LogInformation("Default role(s) created");

                // Create the default users
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment == Environments.Production)
                {
                    logger.LogWarning("Production environment detected, no default users will be created");
                    return;
                }

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                await IdentitySeed.SeedDefaultUsersAsync(userManager);
                logger.LogInformation("Default user(s) created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing identity's content");
                throw;
            }
        }
    }
}
