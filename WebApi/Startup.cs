using System;
using Application;
using Application.Commons.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WebApi.Commons.ExceptionFilters;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        /// <summary>
        /// Name of the CORS policy allowing everything, intended for the development environment
        /// </summary>
        private const string CorsAllowAll = "CorsAllowAll";

        /// <summary>
        /// Name of the CORS policy build from the appsettings.json, intended for the other environments
        /// </summary>
        private const string CorsAllowSpecific = "CorsAllowSpecific";

        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddApplication();

            services.AddTransient<ICurrentUserService, CurrentUserService>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCustomServices(services);

            services.AddControllers(options =>
            {
                // By default, asynchronous endpoints will have the '-Async' part of their name omitted
                // see: https://docs.microsoft.com/en-us/dotnet/core/compatibility/aspnetcore#mvc-async-suffix-trimmed-from-controller-action-names
                options.SuppressAsyncSuffixInActionNames = false;

                // Add filters for the application's exceptions
                options.Filters.Add<ApplicationExceptionFilter>();
                options.Filters.Add<IdentityExceptionFilter>();
                options.Filters.Add<NotFoundExceptionFilter>();
                options.Filters.Add<UnauthorizedExceptionFilter>();
                options.Filters.Add<ValidationExceptionFilter>();
            });

            services.AddCors(options =>
            {
                // Build the default policy for the dev environment, allowing everything
                options.AddPolicy(CorsAllowAll, build
                    => build.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin());

                // Build the specific policy for the other environments with details specified in the appsettings.json
                var specificOrigins = Configuration.GetSection("Cors:Origins")
                    .Get<string[]>();

                options.AddPolicy(CorsAllowSpecific, build 
                    => build.WithOrigins(specificOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock management web API", Version = "v1" });
                
                swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below:",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey,
                });

                swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Name = "Bearer",
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                        },
                        new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);

                swaggerOptions.IncludeXmlComments(commentsFile);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseInfrastructure();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options 
                    => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock management web API v1"));
            }

            app.UseCors(env.IsDevelopment()
                ? CorsAllowAll
                : CorsAllowSpecific);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
