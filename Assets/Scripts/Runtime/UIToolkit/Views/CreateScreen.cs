using System;
using Runtime.Models;
using Runtime.UIToolkit.Extensions;
using Runtime.UIToolkit.ViewModels;
using Unity.AppUI.Core;
using Unity.AppUI.UI;
using Unity.Properties;
using UnityEngine.UIElements;
using TextField = Unity.AppUI.UI.TextField;

namespace Runtime.UIToolkit.Views
{
    public class CreateScreen : BaseNavigationScreen<CreateScreenViewModel>
    {
        private Unity.AppUI.UI.TextField _releaseNameField;
        private Unity.AppUI.UI.DateRangeField _dateRangeField;
        private Unity.AppUI.UI.IntField _taskIntField;
        private Unity.AppUI.UI.Dropdown _daysDropdown;
        private Unity.AppUI.UI.Button _createButton;

        public CreateScreen(CreateScreenViewModel bindingContext) : base(bindingContext)
        {
        }


        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _releaseNameField = this.Q<TextField>("releaseNameField");
            _dateRangeField = this.Q<DateRangeField>("dateRangePicker");
            _taskIntField = this.Q<IntField>("totalTasksField");
            _daysDropdown = this.Q<Dropdown>("checkInDayDropdown");
            _createButton = this.Q<Unity.AppUI.UI.Button>("createButton");

            dataSource = BindingContext;

            _releaseNameField.SetBinding(BindingMode.TwoWay,
                PropertyPath.FromName(nameof(CreateScreenViewModel.ReleaseName)));

            _taskIntField.SetBinding(BindingMode.TwoWay,
                PropertyPath.FromName(nameof(CreateScreenViewModel.TasksNumber)));

            _daysDropdown.SetBinding(BindingMode.TwoWay,
                PropertyPath.FromName(nameof(CreateScreenViewModel.SelectedDay)));

            _dateRangeField.SetBinding(BindingMode.TwoWay,
                PropertyPath.FromName(nameof(CreateScreenViewModel.DateRange)));


            _daysDropdown.bindItem = (item, index) => item.label = BindingContext.DaysOptions[index];
            _daysDropdown.sourceItems = BindingContext.DaysOptions;

            _createButton.clicked += BindingContext.CreateCommand.Execute;
            BindingContext.PropertyChanged += (obj, evt) =>
            {
                if (evt.PropertyName == nameof(BindingContext.Status))
                    OnNotification(BindingContext.Status);
            };
        }

        private void OnNotification(string release)
        {
            var toast = Toast.Build(this, $"Release created: {release}", NotificationDuration.Short)
                .SetStyle(NotificationStyle.Informative)
                .SetPosition(PopupNotificationPlacement.BottomRight)
                .SetAnimationMode(AnimationMode.Slide)
                .SetIcon("info")
                .AddAction(DismissAction, "Dismiss", _ => { });

            toast.Show();
        }

        private const int DismissAction = -1;
    }
}