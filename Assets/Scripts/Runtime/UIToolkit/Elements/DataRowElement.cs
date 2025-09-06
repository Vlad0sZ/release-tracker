using System;
using System.Text;
using R3;
using Runtime.Interfaces.UI;
using Runtime.Models;
using Unity.AppUI.Core;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;
using TextField = Unity.AppUI.UI.TextField;

namespace Runtime.UIToolkit.Elements
{
    public sealed class DataRowElement : VisualElement, IBindable<ReleaseDataRow>, IBindable<ReleaseInfo>, IDisposable
    {
        private readonly TextField _dateField;
        private readonly TextField _planField;
        private readonly TextField _percentageField;
        private readonly IntField _numberField;
        private readonly StringBuilder _sb;

        public readonly Subject<ReleaseDataRow> OnChanged = new();

        private ReleaseDataRow _currentRow;
        private ReleaseInfo _releaseInfo;

        public bool IsReadOnly
        {
            get => _numberField.Q<UnityEngine.UIElements.TextField>(IntField.inputUssClassName).isReadOnly;

            set => _numberField.Q<UnityEngine.UIElements.TextField>(IntField.inputUssClassName).isReadOnly = value;
        }

        public Optional<int> MinValue
        {
            get => _numberField.lowValue;

            set => _numberField.lowValue = value;
        }

        public Optional<int> MaxValue
        {
            get => _numberField.highValue;

            set => _numberField.highValue = value;
        }

        private int TotalTasks => _releaseInfo?.TotalTasks ?? 0;

        public DataRowElement(VisualTreeAsset template)
        {
            template.CloneTree(this);
            _dateField = this.Q<TextField>("dateField");
            _planField = this.Q<TextField>("planField");
            _percentageField = this.Q<TextField>("percentageField");
            _numberField = this.Q<IntField>("numberField");
            _sb = new StringBuilder();
            _numberField.RegisterValueChangedCallback(OnChangedValue);
        }

        public void Bind(ReleaseInfo data)
        {
            _releaseInfo = data;
            _numberField.lowValue = 0;
            _numberField.highValue = TotalTasks;
        }

        public void Bind(ReleaseDataRow data)
        {
            _currentRow = data;
            _dateField.value = DateTime.Parse(data.Date).ToString("d");
            _planField.value = data.Plan.ToString();
            _numberField.SetValueWithoutNotify(data.Fact);
            UpdatePercentage();
        }

        private void OnChangedValue(ChangeEvent<int> evt)
        {
            if (_currentRow == null)
                return;

            _currentRow.Fact = evt.newValue;
            UpdatePercentage();
            OnChanged.OnNext(_currentRow);
        }


        private void UpdatePercentage()
        {
            float percentage = _currentRow.GetDeviationPercent(TotalTasks);
            var sign = Mathf.Sign(percentage) > 0 ? "+" : "-";
            var absPercentage = Mathf.Abs(percentage);

            _sb.Clear();
            if (absPercentage != 0)
                _sb.Append(sign);

            _sb.Append(absPercentage.ToString("F1")).Append("%");
            _percentageField.value = _sb.ToString();
        }

        public void Dispose()
        {
            OnChanged.OnCompleted();
            OnChanged?.Dispose();
        }
    }
}