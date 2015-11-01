using FubuMVC.Core;
using Jajo.Cms.Menu;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class BreadCrumbsRegistration : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(x => x.SetServiceIfNone<IFindBreadCrumbsFor, DefaultBreadCrumbsFinder>());
        }
    }
}