using System.Threading.Tasks;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Parsing
{
    public interface ITextParser
    {
        Task<string> Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme);
    }
}