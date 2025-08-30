using System;
using Runtime.Interfaces.Services;
using Unity.AppUI.MVVM;
using Unity.AppUI.Navigation;

namespace Runtime.Navigation
{
    [Serializable]
    public class DependencyDestinationTemplate : DefaultDestinationTemplate
    {
        public override INavigationScreen CreateScreen(NavHost host)
        {
            NavigationScreen screen = null;
            var type = Type.GetType(template);
            if (type == null)
            {
                var msg = string.IsNullOrEmpty(template)
                    ? "The template type is not set."
                    : $"The template type '{template}' is not valid.";

                UnityEngine.Debug.LogWarning($"{msg} Falling back to default screen type.");
                screen = new NavigationScreen();
            }

            // TODO temporary solution as navigation does not support DI
            else if (App.current.services is ServiceProvider serviceProvider)
            {
                using var scope = serviceProvider.CreateScope();
                var destinationFactory = scope.ServiceProvider.GetRequiredService<IDestinationFactory>();
                screen = destinationFactory.CreateDestination(type);
            }
            else
            {
                screen = Activator.CreateInstance(type) as NavigationScreen;
            }


            if (screen == null)
            {
                throw new InvalidOperationException($"The template '{template}' could not be instantiated. " +
                                                    "Ensure that the type is a valid NavigationScreen and is accessible " +
                                                    "and has a parameterless constructor.");
            }

            screen.showBottomNavBar = showBottomNavBar;
            screen.showAppBar = showAppBar;
            screen.showBackButton = showBackButton;
            screen.showDrawer = showDrawer;
            screen.showNavigationRail = showNavigationRail;

            return screen;
        }
    }
}