using System;
using Runtime.UIToolkit.Extensions;
using Runtime.UIToolkit.ViewModels;
using Unity.AppUI.Core;
using Unity.AppUI.UI;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Views
{
    public class CreateScreen : BaseNavigationScreen<CreateScreenViewModel>
    {
        private Unity.AppUI.UI.TextField _releaseNameField;
        private DateRangeField _dateRangeField;
        private IntField _taskIntField;
        private Dropdown _daysDropdown;
        private Unity.AppUI.UI.Button _createButton;

        public CreateScreen(CreateScreenViewModel bindingContext) : base(bindingContext)
        {
        }


        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _releaseNameField = this.Q<Unity.AppUI.UI.TextField>("releaseNameField");
            _dateRangeField = this.Q<DateRangeField>("dateRangePicker");
            _taskIntField = this.Q<IntField>("totalTasksField");
            _daysDropdown = this.Q<Dropdown>("checkInDayDropdown");
            _createButton = this.Q<Unity.AppUI.UI.Button>("createButton");

            dataSource = BindingContext;

            SetupBindings();
            SetupValidation();
            SetupDropdown();

            _createButton.clicked += OnCreateClicked;

            BindingContext.PropertyChanged += (_, evt) =>
            {
                if (evt.PropertyName == nameof(BindingContext.Status))
                    OnNotification(BindingContext.Status);
            };
        }

        private void SetupBindings()
        {
            _releaseNameField.SetBinding(BindingMode.TwoWay,
                PropertyPath.FromName(nameof(CreateScreenViewModel.ReleaseName)));


            _taskIntField.SetBinding(BindingMode.TwoWay,
                PropertyPath.FromName(nameof(CreateScreenViewModel.TasksNumber)));

            _daysDropdown.SetBinding(BindingMode.TwoWay,
                PropertyPath.FromName(nameof(CreateScreenViewModel.SelectedDay)));

            _dateRangeField.SetBinding(BindingMode.ToTarget,
                PropertyPath.FromName(nameof(CreateScreenViewModel.DateRange)));

            _dateRangeField.RegisterValueChangedCallback(evt =>
                BindingContext.DateRange = evt.newValue
            );
        }

        private void SetupValidation()
        {
            _releaseNameField.validateValue = s => string.IsNullOrEmpty(s) == false && s.Length > 2;

            _taskIntField.validateValue = i => i > 0;

            _dateRangeField.validateValue = range =>
            {
                var different = (DateTime) range.end - (DateTime) range.start;
                return different.TotalDays > 3;
            };

            _dateRangeField.formatString = "d";
        }

        private void SetupDropdown()
        {
            _daysDropdown.bindItem = (item, index) => item.label = BindingContext.DaysOptions[index];
            _daysDropdown.sourceItems = BindingContext.DaysOptions;
        }


        private void OnCreateClicked()
        {
            if (_releaseNameField.invalid || _taskIntField.invalid || _dateRangeField.invalid)
                return;

            BindingContext.CreateCommand.Execute();
        }

        private void OnNotification(string release)
        {
            var toast = Toast.Build(this, $"@UI:toast.create.text", NotificationDuration.Long)
                .SetStyle(NotificationStyle.Informative)
                .SetPosition(PopupNotificationPlacement.BottomRight)
                .SetAnimationMode(AnimationMode.Slide)
                .SetIcon("info")
                .SetVariables(release)
                .AddAction(DismissAction, "@UI:toast.dismiss.text", _ => { });

            toast.Show();
        }

        private const int DismissAction = -1;
    }
}