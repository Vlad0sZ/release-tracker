using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Runtime.Commands;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.Logging;
using Runtime.Interfaces.UI;
using Runtime.Models;
using Unity.AppUI.Core;
using Unity.AppUI.MVVM;
using Unity.AppUI.Navigation;
using Unity.AppUI.UI;
using Unity.Properties;

namespace Runtime.UIToolkit.ViewModels
{
    [ObservableObject]
    [UsedImplicitly]
    public partial class CreateScreenViewModel : IViewModel
    {
        private readonly ILogger<CreateScreenViewModel> _logger;
        private readonly IDataContainer _dataContainer;
        private readonly NavHost _navHost;

        [ObservableProperty] private DateRange _dateRange;

        [ObservableProperty] private int _tasksNumber;

        [ObservableProperty] private int _selectedDay;

        [ObservableProperty] private string _releaseName;

        [ObservableProperty] private string _status;

        [CreateProperty(ReadOnly = true)] public List<string> DaysOptions { get; }

        [CreateProperty(ReadOnly = true)] public RelayCommand CreateCommand { get; }

        public CreateScreenViewModel(NavHost navHost, IDataContainer dataContainer,
            ILogger<CreateScreenViewModel> logger)
        {
            _navHost = navHost;
            _dataContainer = dataContainer;
            _logger = logger;

            var now = DateTime.Now;
            _dateRange = new DateRange(
                now.AddDays(1), now.AddDays(7)
            );

            _tasksNumber = 100;
            _selectedDay = 0;
            _releaseName = "";

            CreateCommand = new RelayCommand(CreateRelease);
            DaysOptions = new List<string>() {"пн", "вт", "ср", "чт", "пт", "сб", "вс"};
        }

        private void CreateRelease()
        {
            var release = new ReleaseInfo()
            {
                TotalTasks = this.TasksNumber,
                CheckIn = this.SelectedDay,
                Name = this.ReleaseName,
                StartDate = ((DateTime) this.DateRange.start).ToString("yyyy-MM-dd"),
                EndDate = ((DateTime) this.DateRange.end).ToString("yyyy-MM-dd"),
            };

            _logger.LogInfo($"Create new release {release}");
            _dataContainer.Data.Add(release);
            Status = release.Name;

            _navHost.navController.PopBackStack();
        }
    }
}