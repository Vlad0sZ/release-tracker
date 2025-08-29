using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Runtime.Interfaces.Loaders;
using Runtime.Interfaces.Navigations;
using Runtime.UI.Screens;
using VContainer.Unity;

namespace Runtime
{
    [UsedImplicitly]
    internal sealed class EntryPoint : IAsyncStartable
    {
        private readonly IEnumerable<IInitializeLoader> _loaders;
        private readonly INavigationController _navigationController;

        public EntryPoint(IEnumerable<IInitializeLoader> loaders, INavigationController navigationController)
        {
            _loaders = loaders;
            _navigationController = navigationController;
        }


        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            foreach (var loader in _loaders)
            {
                await loader.Initialize(cancellation);
            }

            _navigationController.NavigateTo<StartScreenController>();
        }
    }
}