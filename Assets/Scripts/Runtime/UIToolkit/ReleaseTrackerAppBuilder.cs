using Runtime.Interfaces.Services;
using Runtime.Services;
using Runtime.UIToolkit.Extensions;
using Runtime.UIToolkit.Views;
using Unity.AppUI.MVVM;
using Unity.AppUI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit
{
    public sealed class ReleaseTrackerAppBuilder : UIToolkitAppBuilder<ReleaseTrackerApp>
    {
        [SerializeField] private VisualTreeAsset[] templates;
        [SerializeField] private NavGraphViewAsset navigationGraph;

        protected override void OnAppInitialized(ReleaseTrackerApp app)
        {
            base.OnAppInitialized(app);
            Debug.Log("ReleaseTrackerAppBuilder.OnAppInitialized");
        }

        protected override void OnConfiguringApp(AppBuilder builder)
        {
            base.OnConfiguringApp(builder);
            Debug.Log("ReleaseTrackerAppBuilder.OnConfiguringApp");


            AssetProvider.TemplateAssets = templates;
            AssetProvider.NavGraph = navigationGraph;

            builder.services
                .AddIOServices()
                .AddLogging();


            // Add services here

            // Add ViewModels and Views as services
            // builder.services.AddSingleton<MainViewModel>();
            // builder.services.AddSingleton<MainPage>();

            builder.services.AddSingleton<NavHost>();
            builder.services.AddSingleton<IDestinationFactory, DestinationFactory>();
            builder.services.AddSingleton<INavVisualController, NavigationController>();
            builder.services.AddSingleton<ITemplateLoader, VisualTreeAssetLoader>();

            builder.services.AddTransient<LoadingPage>();
        }

        protected override void OnAppShuttingDown(ReleaseTrackerApp app)
        {
            base.OnAppShuttingDown(app);
            Debug.Log("ReleaseTrackerAppBuilder.OnAppShuttingDown");
        }
    }
}