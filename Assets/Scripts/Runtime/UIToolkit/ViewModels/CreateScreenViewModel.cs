using System;
using JetBrains.Annotations;
using Runtime.Interfaces.Logging;
using Runtime.Interfaces.UI;
using Unity.AppUI.MVVM;

namespace Runtime.UIToolkit.ViewModels
{
    [ObservableObject]
    [UsedImplicitly]
    public partial class CreateScreenViewModel : IViewModel
    {
        private readonly ILogger<CreateScreenViewModel> _logger;

        [ObservableProperty] private DateTime _startedDate;

        [ObservableProperty] private DateTime _endDate;

        [ObservableProperty] private int _tasksNumber;

        [ObservableProperty] private int _selectedDay;

        [ObservableProperty] private string _releaseName;

        public CreateScreenViewModel(ILogger<CreateScreenViewModel> logger)
        {
            var now = DateTime.Now;
            _startedDate = now.AddDays(1);
            _endDate = now.AddDays(7);

            _tasksNumber = 100;
            _selectedDay = 0;
            _releaseName = "";
        }
    }
}