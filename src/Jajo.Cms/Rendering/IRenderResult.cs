using System.IO;
using System.Threading.Tasks;

namespace Jajo.Cms.Rendering
{
    public interface IRenderResult
    {
        string ContentType { get; }
        Task RenderTo(TextWriter writer);
    }
}