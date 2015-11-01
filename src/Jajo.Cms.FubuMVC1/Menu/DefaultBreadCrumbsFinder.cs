using System.Collections.Generic;
using System.Collections.ObjectModel;
using FubuCore;
using FubuMVC.Core.Runtime;
using Jajo.Cms.Menu;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class DefaultBreadCrumbsFinder : IFindBreadCrumbsFor
    {
        private readonly IFubuRequest _fubuRequest;
        private readonly IServiceLocator _serviceLocator;

        public DefaultBreadCrumbsFinder(IFubuRequest fubuRequest, IServiceLocator serviceLocator)
        {
            _fubuRequest = fubuRequest;
            _serviceLocator = serviceLocator;
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

            return new ReadOnlyCollection<BreadCrumb>(breadCrumbs);
        }
    }
}