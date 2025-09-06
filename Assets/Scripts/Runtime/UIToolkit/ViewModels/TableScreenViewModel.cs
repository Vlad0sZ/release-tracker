using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Commands;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.Logging;
using Runtime.Interfaces.UI;
using Runtime.Models;
using Unity.AppUI.MVVM;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.ViewModels
{
    [ObservableObject]
    public partial class TableScreenViewModel : IViewModel
    {
        private readonly IDataContainer _dataContainer;

        private readonly ILogger<TableScreenViewModel> _logger;

        private readonly IApp<UIToolkitHost> _app;

        [ObservableProperty] private ReleaseInfo _release;

        [ObservableProperty] private bool _isLoading;
        [CreateProperty(ReadOnly = true)] public IAsyncRelayCommand SaveCommand { get; }

        [CreateProperty(ReadOnly = true)] public IAsyncRelayCommand ShowAnimationCommand { get; }

        public string ReleaseId
        {
            set => SetRelease(value);
        }

        public TableScreenViewModel(IDataContainer dataContainer, ILogger<TableScreenViewModel> logger)
        {
            _logger = logger;
            _dataContainer = dataContainer;
            SaveCommand = new AsyncUniRelayCommand(SaveReleaseData);
            ShowAnimationCommand = new AsyncUniRelayCommand(ShowAnimation);
        }

        private void SetRelease(string releaseId)
        {
            IsLoading = true;

            var release = _dataContainer.Data.SingleOrDefault(x => x.Id == releaseId);
            if (release != null)
                Release = release;

            IsLoading = false;
        }

        private async UniTask SaveReleaseData(CancellationToken cancellationToken)
        {
            try
            {
                IsLoading = true;
                await _dataContainer.Update(Release, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async UniTask ShowAnimation()
        {
            App.current.rootVisualElement.style.display = DisplayStyle.None;
            await UniTask.Delay(4000);
            App.current.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}