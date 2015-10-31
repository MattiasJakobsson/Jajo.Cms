using System.Threading.Tasks;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Parsing
{
    public interface ITextParser
    {
        Task<string> Parse(string text, ICmsContext context, ITheme theme);
    }
}