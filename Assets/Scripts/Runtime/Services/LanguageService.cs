using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Runtime.Interfaces.Loaders;
using Runtime.Interfaces.Services;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Runtime.Services
{
    [UsedImplicitly]
    public sealed class LanguageService : ILanguageSettingsService, IInitializeLoader
    {
        private IList<Locale> _availableLocales;
        private Locale _selectedLocale;
        private const string PlayerPreferenceSettings = "@Language";

        public async UniTask Initialize(CancellationToken cancellationToken)
        {
            _availableLocales = LocalizationSettings.AvailableLocales.Locales.ToList();
            _selectedLocale = await LocalizationSettings.SelectedLocaleAsync.Task;
        }

        public List<string> GetAvailableItems() => _availableLocales.Select(x => x.LocaleName).ToList();

        public int SelectedIndex
        {
            get
            {
                if (_availableLocales == null || _availableLocales.Count == 0 || _selectedLocale == null)
                    return 0;

                return _availableLocales.IndexOf(_selectedLocale);
            }

            set
            {
                if (_availableLocales == null || _availableLocales.Count == 0)
                    return;

                var locale = _availableLocales.ElementAtOrDefault(value);
                if (locale == null || locale == _selectedLocale)
                    return;

                _selectedLocale = locale;
                UnityEngine.PlayerPrefs.SetString(PlayerPreferenceSettings, locale.Identifier.Code);
                LocalizationSettings.SelectedLocale = locale;
            }
        }
    }
}