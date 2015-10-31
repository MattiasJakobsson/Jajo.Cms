using System.Collections.Generic;

namespace Jajo.Cms.Features
{
    public interface IBelongToFeatures
    {
        IEnumerable<string> GetFeatures();
    }
}