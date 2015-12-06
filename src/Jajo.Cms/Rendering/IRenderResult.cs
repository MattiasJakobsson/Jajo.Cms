using System.IO;

namespace Jajo.Cms.Rendering
{
    public interface IRenderResult
    {
        string ContentType { get; }
        void RenderTo(TextWriter writer);
        string Read();
    }
}