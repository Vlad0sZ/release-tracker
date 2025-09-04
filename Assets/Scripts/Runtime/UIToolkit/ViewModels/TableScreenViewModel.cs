using System.Linq;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.Logging;
using Runtime.Interfaces.UI;
using Runtime.Models;
using Unity.AppUI.MVVM;
using Unity.Properties;

namespace Runtime.UIToolkit.ViewModels
{
    [ObservableObject]
    public partial class TableScreenViewModel : IViewModel
    {
        private readonly IDataContainer _dataContainer;

        private readonly ILogger<TableScreenViewModel> _logger;

        [ObservableProperty] private ReleaseInfo _release;

        public string ReleaseId
        {
            set => SetRelease(value);
        }

        public TableScreenViewModel(IDataContainer dataContainer, ILogger<TableScreenViewModel> logger)
        {
            _logger = logger;
            _dataContainer = dataContainer;
        }

        private void SetRelease(string releaseId)
        {
            var release = _dataContainer.Data.SingleOrDefault(x => x.Id == releaseId);
            if (release != null)
                Release = release;
        }
    }
}