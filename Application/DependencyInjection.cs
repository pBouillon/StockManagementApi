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
        }
    }
}
