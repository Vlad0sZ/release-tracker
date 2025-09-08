using System.Collections;
using System.Collections.Generic;

namespace Runtime.Interfaces.Services
{
    public interface ISettings
    {
        public List<string> GetAvailableItems();

        public int SelectedIndex { get; set; }
    }

    public interface ILanguageSettingsService : ISettings
    {
    }

    public interface IThemeSettingsService : ISettings
    {
    }
}