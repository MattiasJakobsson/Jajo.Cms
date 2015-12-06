using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;
using MarkdownSharp;

namespace Jajo.Cms.Parsing
{
    public class MarkdowTextParser : RegexTextParser
    {
        public override IEnumerable<string> GetTags()
        {
            yield return "markdown";
            yield return "tohtml";
            yield return "singletarget";
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme, Func<string, string> recurse)
        {
            var markdownParser = new Markdown(new MarkdownOptions
            {
                AutoNewLines = true
            });

            var mardown = match.Groups[1].Value;

            if (string.IsNullOrEmpty(mardown))
                return mardown;

            return markdownParser.Transform(mardown);
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\<md\>(.*?)\<\/md\>", RegexOptions.Compiled | RegexOptions.Singleline);
        }
    }
}