using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RuhRoh.Extensions.Microsoft.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace RuhRoh
{
    /// <summary>
    /// Extension method class to get started when configuring services to affect at runtime.
    /// </summary>
    public static class ChaosEngineExtensions
    {
        /// <summary>
        /// Register a service and affect its behavior.
        /// </summary>
        /// <typeparam name="TService">Type of the service.</typeparam>
        /// <typeparam name="TImplementation">Implementation of the service.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> containing the service to affect.</param>
        public static AffectedType<TService> AffectScoped<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            var typeToAffect = typeof(TService);
            
            // Create a new AffectedType<T> (or AffectedService because we need dependency injection)
            var affectedType = new AffectedService<TService, TImplementation>();

            var registration =
                services.FirstOrDefault(x => x.ServiceType == typeToAffect && x.ImplementationType == typeof(TImplementation));
            
            if (registration == null)
            {
                services.AddScoped(sp => affectedType.GetInstance(sp));
            }
            else
            {
                services.Replace(
                    new ServiceDescriptor(typeToAffect, sp => affectedType.GetInstance(sp), ServiceLifetime.Scoped));
            }

            // Register the implementation of TService, we need to be able to resolve it in the AffectedService.
            services.AddScoped<TImplementation>();

            return affectedType;
        }
    }
}
