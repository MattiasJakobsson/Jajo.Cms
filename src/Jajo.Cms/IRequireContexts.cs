using System.Collections.Generic;

namespace Jajo.Cms
{
    public interface IRequireContexts
    {
        IEnumerable<string> GetRequiredContexts();
    }
}