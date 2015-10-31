using System.Threading.Tasks;
using Jajo.Cms.Theme;
using MarkdownSharp;

namespace Jajo.Cms.Parsing
{
    public class MarkdowTextParser : ITextParser
    {
        public Task<string> Parse(string text, ICmsContext context, ITheme theme)
        {
            var markdownParser = new Markdown();

            return Task.FromResult(markdownParser.Transform(text.Replace("\n", "<br/>")));
        }
    }
}