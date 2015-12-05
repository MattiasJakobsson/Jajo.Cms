using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jajo.Cms.Parsing;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Localization
{
    public class LocalizationTextParser : RegexTextParser
    {
        private readonly ILocalizeText _localizeText;
        private readonly IFindCurrentLocalizationNamespace _findCurrentLocalizationNamespace;

        public LocalizationTextParser(ILocalizeText localizeText, IFindCurrentLocalizationNamespace findCurrentLocalizationNamespace)
        {
            _localizeText = localizeText;
            _findCurrentLocalizationNamespace = findCurrentLocalizationNamespace;
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme)
        {
            var localizationNamespace = _findCurrentLocalizationNamespace.Find();

            var key = !string.IsNullOrEmpty(localizationNamespace) ? string.Format("{0}:{1}", localizationNamespace, match.Groups["resource"].Value) : match.Groups["resource"].Value;

            return _localizeText.Localize(key);
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\%\[(?<resource>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9_-]*)\]\%", RegexOptions.Compiled);
        }
    }
}