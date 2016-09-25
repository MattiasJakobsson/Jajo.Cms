using System.Collections.Generic;
using System.Globalization;

namespace Jajo.Cms.Localization
{
    public interface ILocalizeText
    {
        string Localize(string key, CultureInfo culture);
        void Load();
        IReadOnlyDictionary<string, string> GetTranslations(CultureInfo culture, string theme);
        void AddMissing(IEnumerable<string> keys, CultureInfo culture);
    }
}