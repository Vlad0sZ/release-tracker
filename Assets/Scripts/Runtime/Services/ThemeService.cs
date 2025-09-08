using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Runtime.Interfaces.Loaders;
using Runtime.Interfaces.Services;
using Unity.AppUI.Core;
using Unity.AppUI.MVVM;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Services
{
    [UsedImplicitly]
    public sealed class ThemeService : IThemeSettingsService, IInitializeLoader
    {
        private const string PlayerPreferenceSettings = "@Theme";
        private Panel _panel;

        private readonly List<string> _values = new() {"dark", "light"};
        private string _selectedValue;

        public UniTask Initialize(CancellationToken cancellationToken)
        {
            _panel = App.current.rootVisualElement.Q<Panel>();

            if (PlayerPrefs.HasKey(PlayerPreferenceSettings))
                _selectedValue = PlayerPrefs.GetString(PlayerPreferenceSettings);
            else
                _selectedValue = Platform.darkMode ? _values[0] : _values[1];

            _panel.theme = _selectedValue;
            return UniTask.CompletedTask;
        }

        public List<string> GetAvailableItems() => _values;

        public int SelectedIndex
        {
            get => _values.IndexOf(_selectedValue);

            set
            {
                var theme = _values.ElementAtOrDefault(value);
                if (string.IsNullOrEmpty(theme) || theme == _selectedValue)
                    return;

                _selectedValue = theme;
                _panel.theme = theme;
                PlayerPrefs.SetString(PlayerPreferenceSettings, theme);
            }
        }
    }
}