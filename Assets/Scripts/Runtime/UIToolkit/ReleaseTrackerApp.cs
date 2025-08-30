using JetBrains.Annotations;
using Unity.AppUI.MVVM;
using Unity.AppUI.Navigation;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit
{
    [UsedImplicitly]
    public sealed class ReleaseTrackerApp : App
    {
        private readonly INavVisualController _navVisualController;
        private readonly NavHost _navHost;

        public ReleaseTrackerApp(NavHost navHost, INavVisualController navVisualController)
        {
            _navVisualController = navVisualController;
            _navHost = navHost;
        }

        public override void InitializeComponent()
        {
            base.InitializeComponent();

            _navHost.navController.SetGraph(AssetProvider.NavGraph);
            _navHost.visualController = _navVisualController;

            var panel = new Unity.AppUI.UI.Panel
            {
                scale = "large"
            };

            rootVisualElement.Add(panel);
            panel.StretchToParentSize();

            panel.Add(_navHost);
            _navHost.StretchToParentSize();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }
    }
}