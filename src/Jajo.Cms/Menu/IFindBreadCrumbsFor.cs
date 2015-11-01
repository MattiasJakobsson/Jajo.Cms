using System.Collections.Generic;

namespace Jajo.Cms.Menu
{
    public interface IFindBreadCrumbsFor
    {
        IEnumerable<BreadCrumb> Get(object input);
    }
}