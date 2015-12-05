using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;
using MarkdownSharp;

namespace Jajo.Cms.Parsing
{
    public class MarkdowTextParser : ITextParser
    {
        public string Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme)
        {
            var markdownParser = new Markdown();

            return markdownParser.Transform(text);
        }
    }
}