using System.Globalization;

namespace Jajo.Cms.Localization
{
    public interface ILocalizeText
    {
        string Localize(string key, CultureInfo culture = null);
    }
}