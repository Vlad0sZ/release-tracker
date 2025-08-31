using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnityApp = Unity.AppUI.MVVM;
using MSApp = Microsoft.Extensions.DependencyInjection;

namespace Unity.AppUI.Extended.DependencyInjection
{
    public sealed class ExtendedAppBuilder : IAppConfiguration
    {
        private readonly IServiceCollection serviceCollection = new ServiceCollection();

        public IServiceCollection Services => serviceCollection;

        /// <summary>
        /// Private constructor to prevent instantiation.
        /// </summary>
        private ExtendedAppBuilder()
        {
        }


        /// <summary>
        /// Instantiates a new AppBuilder with the default services according to the given App type.
        /// </summary>
        /// <typeparam name="TApp"> The type of the app to build. It is expected that this type is a subclass of <see cref="App"/>. </typeparam>
        /// <typeparam name="THost"> The type of the host to use. </typeparam>
        /// <returns> The instantiated AppBuilder. </returns>
        public static ExtendedAppBuilder InstantiateWith<TApp, THost>()
            where TApp : class, UnityApp.IApp<THost>
            where THost : class, UnityApp.IHost
        {
            var builder = new ExtendedAppBuilder();
            builder.serviceCollection.TryAddSingleton<UnityApp.IApp<THost>, TApp>();
            return builder;
        }

        /// <summary>
        /// Build and initialize the app with the given host.
        /// </summary>
        /// <typeparam name="THost"> The type of the host to use. </typeparam>
        /// <param name="host"> The host to use. </param>
        /// <returns> The built app instance. </returns>
        public UnityApp.IApp<THost> BuildWith<THost>(THost host)
            where THost : class, UnityApp.IHost
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<UnityApp.IApp<THost>>();
            app.Initialize(serviceProvider, host);
            return app;
        }
    }
}