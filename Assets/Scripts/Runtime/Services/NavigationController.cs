using JetBrains.Annotations;
using Runtime.Interfaces.Services;
using Runtime.UIToolkit.Elements;
using Unity.AppUI.Navigation;
using Unity.AppUI.UI;

namespace Runtime.Services
{
    [UsedImplicitly]
    public sealed class NavigationController : INavVisualController
    {
        private readonly IThemeSettingsService _themeSettings;
        private readonly ILanguageSettingsService _languageSettings;

        public NavigationController(IThemeSettingsService themeSettings, ILanguageSettingsService languageSettings)
        {
            _themeSettings = themeSettings;
            _languageSettings = languageSettings;
        }

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
            // 

            drawer.swipeAreaWidth = 16;
            drawer.Add(new DrawerHeader());
            drawer.Add(new SettingsDrawer(_languageSettings, _themeSettings));
        }

        public void SetupNavigationRail(NavigationRail navigationRail, NavDestination destination,
            NavController navController)
        {
            // not used.
        }
    }
}