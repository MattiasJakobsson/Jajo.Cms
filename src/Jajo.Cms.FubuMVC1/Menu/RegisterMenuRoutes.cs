using FubuMVC.Core;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class RegisterMenuRoutes : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Route("_menu").Calls<MenuEndpoint>(x => x.MenuQuery(null));
        }
    }
}