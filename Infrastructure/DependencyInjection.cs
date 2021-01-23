using System;
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
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Dependency injection utility for the Infrastructure layer
    /// </summary>
    public static class DependencyInjection
    {
        private static void AddAndConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

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

                // User settings
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

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
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = "http://localhost",
                        ValidIssuer = "http://localhost",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyApplicationKey"))
                    };
                });

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

            AddAndConfigureIdentity(services);

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
