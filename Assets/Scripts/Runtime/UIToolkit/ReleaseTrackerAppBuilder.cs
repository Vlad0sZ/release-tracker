using Microsoft.Extensions.DependencyInjection;
using Runtime.Behaviours;
using Runtime.Containers;
using Runtime.Interfaces.Behaviours;
using Runtime.Interfaces.Services;
using Runtime.Services;
using Runtime.UIToolkit.Extensions;
using Runtime.UIToolkit.ViewModels;
using Runtime.UIToolkit.Views;
using Unity.AppUI.Extended.DependencyInjection;
using Unity.AppUI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit
{
    public sealed class ReleaseTrackerAppBuilder : ExtendedAppHost<ReleaseTrackerApp>
    {
        [SerializeField] private VisualTreeAsset[] templates;
        [SerializeField] private NavGraphViewAsset navigationGraph;
        [SerializeField] private AnimationScreenBehaviour animationScreen;

        protected override void OnAppInitialized(ReleaseTrackerApp app)
        {
            base.OnAppInitialized(app);
            Debug.Log("ReleaseTrackerAppBuilder.OnAppInitialized");
        }

        protected override void OnConfiguringApp(IAppConfiguration appConfiguration)
        {
            base.OnConfiguringApp(appConfiguration);

            appConfiguration.Services
                .AddIOServices()
                .AddLogging();


            appConfiguration.Services.AddSingleton<AssetProvider>(new AssetProvider(templates, navigationGraph));
            appConfiguration.Services.AddSingleton<IAnimationBehaviour>(animationScreen);

            appConfiguration.Services.AddSingleton<NavHost>();
            appConfiguration.Services.AddSingleton<IDestinationFactory, DestinationFactory>();
            appConfiguration.Services.AddSingleton<INavVisualController, NavigationController>();
            appConfiguration.Services.AddSingleton<ITemplateLoader, VisualTreeAssetLoader>();

            appConfiguration.Services.Register<DataContainer>()
                .AsImplementedInterfaces<DataContainer>();

            appConfiguration.Services.Register<LanguageService>()
                .AsImplementedInterfaces<LanguageService>();

            appConfiguration.Services.Register<ThemeService>()
                .AsImplementedInterfaces<ThemeService>();

            appConfiguration.Services.AddScoped<IReleaseGenerator, ReleaseGenerator>();

            appConfiguration.Services.AddTransient<LoadingScreen>();
            appConfiguration.Services.AddTransient<LoadingPageViewModel>();

            appConfiguration.Services.AddTransient<StartScreen>();
            appConfiguration.Services.AddTransient<StartScreenViewModel>();

            appConfiguration.Services.AddTransient<CreateScreen>();
            appConfiguration.Services.AddTransient<CreateScreenViewModel>();

            appConfiguration.Services.AddTransient<TableScreen>();
            appConfiguration.Services.AddTransient<TableScreenViewModel>();
        }

        protected override void OnAppShuttingDown(ReleaseTrackerApp app)
        {
            base.OnAppShuttingDown(app);
            Debug.Log("ReleaseTrackerAppBuilder.OnAppShuttingDown");
        }
    }
}