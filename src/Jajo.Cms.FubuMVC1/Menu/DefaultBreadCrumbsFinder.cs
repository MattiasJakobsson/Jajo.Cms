using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FubuCore;
using FubuMVC.Core.Runtime;
using Jajo.Cms.Menu;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class DefaultBreadCrumbsFinder : IFindBreadCrumbsFor
    {
        private readonly IFubuRequest _fubuRequest;
        private readonly IServiceLocator _serviceLocator;
        private readonly ICmsContext _cmsContext;

        public DefaultBreadCrumbsFinder(IFubuRequest fubuRequest, IServiceLocator serviceLocator, ICmsContext cmsContext)
        {
            _fubuRequest = fubuRequest;
            _serviceLocator = serviceLocator;
            _cmsContext = cmsContext;
        }

        public IEnumerable<BreadCrumb> Get(object input)
        {
            var amBreadCrumb = input as IAmBreadCrumb;

            var breadCrumbs = new List<BreadCrumb>();

            if (amBreadCrumb == null)
                return breadCrumbs;

            var menuContext = new DefaultMenuContext(_fubuRequest, _serviceLocator);

            while (amBreadCrumb != null)
            {
                breadCrumbs.Add(amBreadCrumb.Build(menuContext));

                amBreadCrumb = amBreadCrumb.FindParent() as IAmBreadCrumb;
            }

            breadCrumbs.Reverse();

            breadCrumbs = _cmsContext.Filter(breadCrumbs, _cmsContext.GetCurrentTheme()).ToList();

            return new ReadOnlyCollection<BreadCrumb>(breadCrumbs);
        }
    }
}