using Application.Commons.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Dependency injection utility for the Infrastructure layer
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddAndConfigureIdentity(IServiceCollection services, IConfiguration configuration)
        {
            // Initialize Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>();

            // Register the Identity configuration from appsettings.json
            var section = configuration.GetSection(nameof(IdentityConfiguration));
            var identityConfiguration = section.Get<IdentityConfiguration>();

            services.AddSingleton(identityConfiguration);

            // Configure the Identity options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            // Configure the Identity authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = identityConfiguration.TokenAudience,
                        ValidIssuer = identityConfiguration.TokenIssuer,
                        IssuerSigningKey = identityConfiguration.SecurityKey
                    };
                });

            // Register the associated services
            services.AddScoped<IIdentityService, IdentityService>();
        }
        
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

            AddAndConfigureIdentity(services, configuration);

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
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("StockManagementDatabase"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options
                    => options.UseSqlite(
                        configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}
