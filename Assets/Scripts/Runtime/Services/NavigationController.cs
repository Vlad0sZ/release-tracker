using JetBrains.Annotations;
using Unity.AppUI.Navigation;
using Unity.AppUI.UI;

namespace Runtime.Services
{
    [UsedImplicitly]
    public sealed class NavigationController : INavVisualController
    {
        public void SetupBottomNavBar(BottomNavBar bottomNavBar, NavDestination destination,
            NavController navController)
        {
            // not used.
        }

        public void SetupAppBar(AppBar appBar, NavDestination destination, NavController navController)
        {
            appBar.title = destination.label;
            appBar.stretch = true;
            appBar.expandedHeight = 92;
        }

        public void SetupDrawer(Drawer drawer, NavDestination destination, NavController navController)
        {
            // not used.
        }

        public void SetupNavigationRail(NavigationRail navigationRail, NavDestination destination,
            NavController navController)
        {
            // not used.
        }
    }
}