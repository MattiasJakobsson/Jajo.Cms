using System.Collections.Generic;

namespace Jajo.Cms.Rendering
{
    public interface IRenderInformation
    {
         IEnumerable<IRequestContext> Contexts { get; }
    }
}