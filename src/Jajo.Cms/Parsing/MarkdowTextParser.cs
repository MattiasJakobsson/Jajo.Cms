using System;
using System.Collections.Generic;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;
using MarkdownSharp;

namespace Jajo.Cms.Parsing
{
    public class MarkdowTextParser : ITextParser
    {
        public string Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme, Func<string, string> recurse)
        {
            var markdownParser = new Markdown();

            return markdownParser.Transform(text.Replace("\r\n", "<br/>"));
        }

        public IEnumerable<string> GetTags()
        {
            yield return "markdown";
            yield return "tohtml";
            yield return "singletarget";
        }
    }
}