using System.Linq;
using Runtime.Core;
using Runtime.Interfaces.Navigations;
using Runtime.Navigation;
using Runtime.UI.Screens;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Runtime.Scopes
{
    internal sealed class BootstrapLifetimeScope : LifetimeScope
    {
        [SerializeField] private NavigationScreenBase[] navigationScreens;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<DataContainer>(Lifetime.Scoped)
                .AsImplementedInterfaces();

            foreach (var screen in navigationScreens)
            {
                builder.RegisterComponent(screen)
                    .As<INavigationScreen>();
            }

            builder.Register<INavigationController, NavigationController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<EntryPoint>();
        }
    }
}