using System.Collections.Generic;
using System.Linq;
using Runtime.Interfaces.Services;
using Unity.AppUI.UI;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Elements
{
    public sealed class SettingsDrawer : VisualElement
    {
        private readonly ILanguageSettingsService _languageSettings;
        private readonly IThemeSettingsService _themeSettings;

        public SettingsDrawer(ILanguageSettingsService languageService, IThemeSettingsService themeService)
        {
            _languageSettings = languageService;
            _themeSettings = themeService;

            var inputLanguageLabel = CreateInputLabel("@UI:drawer.language.label");
            var inputThemeLabel = CreateInputLabel("@UI:drawer.theme.label");

            var languages = _languageSettings.GetAvailableItems();
            var themes = _themeSettings.GetAvailableItems();


            var languageDropdown = new Dropdown()
            {
                bindItem = (e, i) => e.label = languages[i],
                sourceItems = languages,
                selectedIndex = _languageSettings.SelectedIndex,
            };


            var themeDropdown = new Dropdown()
            {
                bindItem = (e, i) => e.label = themes[i],
                sourceItems = themes,
                selectedIndex = _themeSettings.SelectedIndex
            };

            inputThemeLabel.Add(themeDropdown);
            inputLanguageLabel.Add(languageDropdown);
            this.style.marginLeft = new StyleLength(Length.Pixels(8));
            this.style.marginRight = new StyleLength(Length.Pixels(8));
            hierarchy.Add(inputLanguageLabel);
            hierarchy.Add(inputThemeLabel);
            languageDropdown.RegisterValueChangedCallback(OnLanguageChanged);
            themeDropdown.RegisterValueChangedCallback(OnThemeChanged);
        }

        private void OnLanguageChanged(ChangeEvent<IEnumerable<int>> change)
        {
            var value = change.newValue.FirstOrDefault();
            _languageSettings.SelectedIndex = value;
        }

        private void OnThemeChanged(ChangeEvent<IEnumerable<int>> change)
        {
            var value = change.newValue.FirstOrDefault();
            _themeSettings.SelectedIndex = value;
        }

        private static VisualElement CreateInputLabel(string label)
        {
            return new InputLabel()
            {
                style =
                {
                    marginBottom = 16
                },

                direction = Direction.Vertical,
                label = label,
            };
        }
    }
}