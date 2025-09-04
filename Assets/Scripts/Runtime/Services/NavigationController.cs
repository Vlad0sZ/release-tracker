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
            appBar.stretch = true;
            appBar.expandedHeight = 64;
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