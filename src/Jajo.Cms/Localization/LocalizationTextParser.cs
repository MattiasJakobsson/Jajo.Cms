using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jajo.Cms.Parsing;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;
using UStack.Infrastructure;

namespace Jajo.Cms.Localization
{
    public class LocalizationTextParser : RegexTextParser
    {
        private readonly ILocalizeText _localizeText;
        private readonly IFindCurrentLocalizationNamespace _findCurrentLocalizationNamespace;
        private readonly IFindCultureForRequest _findCultureForRequest;

        public LocalizationTextParser(ILocalizeText localizeText, IFindCurrentLocalizationNamespace findCurrentLocalizationNamespace, IFindCultureForRequest findCultureForRequest)
        {
            _localizeText = localizeText;
            _findCurrentLocalizationNamespace = findCurrentLocalizationNamespace;
            _findCultureForRequest = findCultureForRequest;
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme)
        {
            var localizationNamespace = _findCurrentLocalizationNamespace.Find();

            var key = !string.IsNullOrEmpty(localizationNamespace) ? string.Format("{0}:{1}", localizationNamespace, match.Groups["resource"].Value) : match.Groups["resource"].Value;

            return _localizeText.Localize(key, _findCultureForRequest.FindUiCulture());
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\%\[(?<resource>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9_-]*)\]\%", RegexOptions.Compiled);
        }
    }
}