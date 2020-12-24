using Application.Commons.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlite(connectionString));

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.AddScoped<IDateTime, DateTimeService>();

        }
    }
}
