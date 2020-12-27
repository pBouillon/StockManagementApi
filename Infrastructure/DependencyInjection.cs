using Application.Commons.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    /// <summary>
    /// Dependency injection utility for the Infrastructure layer
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Add all services of the Application layer
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/> used to setup the dependency injection container
        /// </param>
        /// <param name="configuration">Accessor to the configuration file</param>
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);

            services.AddScoped<IDateTime, DateTimeService>();
        }

        /// <summary>
        /// Setup the database services in the dependency injection container
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/> used to setup the dependency injection container
        /// </param>
        /// <param name="configuration">Accessor to the configuration file</param>
        private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlite(connectionString));

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}
