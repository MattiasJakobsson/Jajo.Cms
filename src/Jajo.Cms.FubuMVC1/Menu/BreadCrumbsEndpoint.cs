using System.Collections.Generic;
using System.Linq;
using Jajo.Cms.Menu;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class BreadCrumbsEndpoint
    {
        private readonly IFindBreadCrumbsFor _findBreadCrumbsFor;

        public BreadCrumbsEndpoint(IFindBreadCrumbsFor findBreadCrumbsFor)
        {
            _findBreadCrumbsFor = findBreadCrumbsFor;
        }

        public BreadCrumbsEndpointQueryResult Query(BreadCrumbsEndpointQueryInput input)
        {
            return new BreadCrumbsEndpointQueryResult(_findBreadCrumbsFor.Get(input.CurrentInput).ToList());
        }
    }

    public class BreadCrumbsEndpointQueryInput
    {
        public object CurrentInput { get; set; }
    }

    public class BreadCrumbsEndpointQueryResult
    {
        public BreadCrumbsEndpointQueryResult(IEnumerable<BreadCrumb> breadCrumbs)
        {
            BreadCrumbs = breadCrumbs;
        }

        public IEnumerable<BreadCrumb> BreadCrumbs { get; private set; }
    }
}