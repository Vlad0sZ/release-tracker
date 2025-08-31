using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Runtime.Commands;
using Runtime.Extensions;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.Loaders;
using Runtime.Interfaces.Logging;
using Runtime.Interfaces.UI;
using Runtime.UIToolkit.Extensions;
using Unity.AppUI.MVVM;
using Unity.AppUI.Navigation;
using Unity.AppUI.Navigation.Generated;
using Unity.Properties;

namespace Runtime.UIToolkit.ViewModels
{
    [UsedImplicitly]
    [ObservableObject]
    public partial class LoadingPageViewModel : IViewModel
    {
        private readonly IEnumerable<IInitializeLoader> _loaders;
        private readonly ILogger<LoadingPageViewModel> _logger;
        private readonly NavHost _navHost;

        /// <summary>
        /// Command for initialization of application;
        /// </summary>
        [CreateProperty(ReadOnly = true)]
        public IAsyncRelayCommand InitializeCommand { get; private set; }

        public LoadingPageViewModel(NavHost navHost, IEnumerable<IInitializeLoader> loaders,
            ILogger<LoadingPageViewModel> logger)
        {
            _loaders = loaders;
            _logger = logger;
            _navHost = navHost;
            InitializeCommand = new AsyncUniRelayCommand(InitializeApp);
        }

        private async UniTask InitializeApp(CancellationToken cancellationToken)
        {
            var loaders = _loaders.ToArray();
            _logger.LogInfo($"Start initialize: total loaders: {loaders.Length}");

            foreach (var loader in loaders)
            {
                _logger.LogInfo($"Start initialize {loader.GetType().Name}");
                await loader.Initialize(cancellationToken);
            }

            _navHost.navController.Navigate(Actions.splash_to_start);
        }
    }
}