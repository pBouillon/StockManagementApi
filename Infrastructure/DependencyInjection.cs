using Application.Commons.Interfaces;
using AspNetCoreRateLimit;
using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Infrastructure
{
    /// <summary>
    /// Dependency injection utility for the Infrastructure layer
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Configure the endpoint throttling
        /// </summary>
        /// <remarks>
        /// More details on the repo:
        /// https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup
        /// </remarks>
        /// <param name="services">
        /// <see cref="IServiceCollection"/> used to setup the dependency injection container
        /// </param>
        /// <param name="configuration">Accessor to the configuration file</param>
        private static void AddEndpointThrottling(this IServiceCollection services, IConfiguration configuration)
        {
            // Needed to load configuration from appsettings.json
            services.AddOptions();

            // Needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            // Load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            // Load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            // Inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // Configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        /// <summary>
        /// Add IdentityServer's services
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/> used to setup the dependency injection container
        /// </param>
        /// <param name="configuration">Accessor to the configuration file</param>
        private static void AddIdentityServer(IServiceCollection services, IConfiguration configuration)
        {
            // Register the Identity configuration from appsettings.json
            var identityConfiguration = configuration
                .GetSection(nameof(IdentityConfiguration))
                .Get<IdentityConfiguration>();

            services.AddSingleton(identityConfiguration);

            // Initialize Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>();

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
                    options.RequireHttpsMetadata = identityConfiguration.TokenRequireHttpsMetadata;
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
        /// Add all services of the Infrastructure layer
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/> used to setup the dependency injection container
        /// </param>
        /// <param name="configuration">Accessor to the configuration file</param>
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointThrottling(configuration);

            services.AddPersistence(configuration);

            AddIdentityServer(services, configuration);

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

        /// <summary>
        /// Use infrastructure's services
        /// </summary>
        /// <param name="app">Application builder</param>
        public static void UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseIpRateLimiting();
        }
    }
}
