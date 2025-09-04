using System.Linq;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.UI;
using Runtime.Models;
using Unity.AppUI.MVVM;
using Unity.AppUI.Navigation;
using Unity.AppUI.Navigation.Generated;
using Unity.Properties;

namespace Runtime.UIToolkit.ViewModels
{
    [ObservableObject]
    public partial class StartScreenViewModel : IViewModel
    {
        private readonly IDataContainer _dataContainer;
        private readonly NavHost _navHost;

        [ObservableProperty] private ReleaseInfo[] _data;

        [CreateProperty(ReadOnly = true)] public RelayCommand CreateCommand { get; }
        [CreateProperty(ReadOnly = true)] public RelayCommand<ReleaseInfo> OpenCommand { get; }
        [CreateProperty(ReadOnly = true)] public RelayCommand<ReleaseInfo> DeleteCommand { get; }
        [CreateProperty(ReadOnly = true)] public RelayCommand<ReleaseInfo> CancelDeleteCommand { get; }


        public StartScreenViewModel(IDataContainer dataContainer, NavHost navHost)
        {
            _dataContainer = dataContainer;
            _navHost = navHost;
            UpdateReleaseData();

            CreateCommand = new RelayCommand(OpenCreateView);
            OpenCommand = new RelayCommand<ReleaseInfo>(OpenReleaseView);
            DeleteCommand = new RelayCommand<ReleaseInfo>(OnDelete);
            CancelDeleteCommand = new RelayCommand<ReleaseInfo>(CancelDelete);
        }

        private void OnDelete(ReleaseInfo obj)
        {
            if (_dataContainer.Data.Remove(obj))
                UpdateReleaseData();
        }

        private void CancelDelete(ReleaseInfo obj)
        {
            if (_dataContainer.Data.Contains(obj))
                return;

            _dataContainer.Data.Insert(0, obj);
            UpdateReleaseData();
        }

        private void OpenCreateView() =>
            _navHost.navController.Navigate(Actions.start_to_create);

        private void OpenReleaseView(ReleaseInfo obj) =>
            _navHost.navController.Navigate(Actions.start_to_table,
                new Argument(Arguments.releaseId, obj.Id));

        private void UpdateReleaseData() =>
            Data = _dataContainer.Data.ToArray();
    }
}