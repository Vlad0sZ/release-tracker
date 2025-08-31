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
        private readonly AssetProvider _assetProvider;
        private readonly NavHost _navHost;

        public ReleaseTrackerApp(NavHost navHost, AssetProvider assetProvider, INavVisualController navVisualController)
        {
            _navVisualController = navVisualController;
            _assetProvider = assetProvider;
            _navHost = navHost;
        }

        public override void InitializeComponent()
        {
            base.InitializeComponent();

            _navHost.navController.SetGraph(_assetProvider.NavGraph);
            _navHost.visualController = _navVisualController;

            rootVisualElement.Add(_navHost);
            _navHost.StretchToParentSize();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }
    }
}