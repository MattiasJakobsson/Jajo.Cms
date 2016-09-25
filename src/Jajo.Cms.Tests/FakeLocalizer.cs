using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Jajo.Cms.Localization;

namespace Jajo.Cms.Tests
{
    public class FakeLocalizer : ILocalizeText
    {
        public string Localize(string key, CultureInfo culture)
        {
            if (key == "test")
                return "asd {{replaceme}} asd";

            return "";
        }

        public void Load()
        {

        }

        public IReadOnlyDictionary<string, string> GetTranslations(CultureInfo culture, string theme)
        {
            return new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
        }
    }
}