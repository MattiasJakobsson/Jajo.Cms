using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Parsing
{
    public interface ITextParser
    {
        string Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme);
    }
}