using System.Collections.Generic;
using Runtime.Core;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.UI;
using Unity.AppUI.MVVM;
using Unity.Properties;

namespace Runtime.UIToolkit.ViewModels
{
    [ObservableObject]
    public partial class StartScreenViewModel : IViewModel
    {
        private readonly IDataContainer _dataContainer;
        [CreateProperty(ReadOnly = true)] public IList<DataClass> Data => _dataContainer.Data;

        public StartScreenViewModel(IDataContainer dataContainer) =>
            _dataContainer = dataContainer;
    }
}