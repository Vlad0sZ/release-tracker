using System;
using Microsoft.Extensions.DependencyInjection;

namespace Runtime.UIToolkit.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Register<TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TImplementation : class
        {
            return services.Register(typeof(TImplementation), lifetime);
        }

        public static IServiceCollection Register(
            this IServiceCollection services,
            Type implementationType,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime));
            return services;
        }

        public static IServiceCollection AsImplementedInterfaces<TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TImplementation : class
        {
            return services.AsImplementedInterfaces(typeof(TImplementation), lifetime);
        }

        public static IServiceCollection AsImplementedInterfaces(
            this IServiceCollection services,
            Type implementationType,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            var interfaces = implementationType.GetInterfaces();

            foreach (var interfaceType in interfaces)
            {
                services.Add(new ServiceDescriptor(
                    interfaceType,
                    serviceProvider => serviceProvider.GetService(implementationType),
                    lifetime
                ));
            }

            return services;
        }
    }
}