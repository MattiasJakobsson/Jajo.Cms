using System.Collections.Generic;

namespace Jajo.Cms.Rendering
{
    public interface IRenderInformation
    {
        string ContentType { get; }
        IEnumerable<IRequestContext> Contexts { get; }
    }
}