using Application.Commons.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    /// <summary>
    /// Dependency injection utility for the Application layer
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Add all services of the Application layer
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/> used to setup the dependency injection container
        /// </param>
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.ConfigureMediatrPipeline();
        }

        /// <summary>
        /// Setup the MediatR validation pipeline
        /// </summary>
        /// <remarks>
        /// The pipeline will be executed in the same order as set up below (from the first one to the last)
        /// </remarks>
        /// <param name="services">
        /// <see cref="IServiceCollection"/> used to setup the dependency injection container
        /// </param>
        private static void ConfigureMediatrPipeline(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
