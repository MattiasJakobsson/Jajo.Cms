using System.Globalization;
using Jajo.Cms.Localization;

namespace Jajo.Cms.FubuMVC1.Localization
{
    public class DefaultTextLocalizer : ILocalizeText
    {
        public string Localize(string key, CultureInfo culture = null)
        {
            return key;
        }
    }
}