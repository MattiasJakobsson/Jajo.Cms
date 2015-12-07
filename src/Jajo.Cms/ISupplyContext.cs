using System.Collections.Generic;

namespace Jajo.Cms
{
    public interface ISupplyContext
    {
        IEnumerable<RequestContext> GetContexts();
    }
}