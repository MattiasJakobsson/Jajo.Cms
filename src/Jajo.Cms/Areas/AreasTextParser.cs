using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Jajo.Cms.Parsing;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Areas
{
    public class AreasTextParser : RegexTextParser
    {
        public override IEnumerable<string> GetTags()
        {
            yield return "areas";
            yield return "advanced";
            yield return "multitarget";
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme, Func<string, string> recurse)
        {
            var areaNameGroup = match.Groups["areaName"];

            if (areaNameGroup == null)
                return "";

            var areaName = areaNameGroup.Value;

            var areaContent = context
                .FindContexts(AreasContext.Name)
                .Select(x => AreasContext.GetAreaFrom(x, areaName))
                .LastOrDefault(x => !string.IsNullOrEmpty(x));

            return string.IsNullOrEmpty(areaContent) ? "" : recurse(areaContent);
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\!\[Area\.((?<areaName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9*))\]\!", RegexOptions.Compiled);
        }
    }
}