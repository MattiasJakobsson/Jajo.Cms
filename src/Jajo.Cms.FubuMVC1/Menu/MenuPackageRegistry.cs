using FubuMVC.Core;
using FubuMVC.Core.Registration;
using Jajo.Cms.Menu;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class MenuPackageRegistry : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(x =>
            {
                x.Scan(y =>
                {
                    y.Applies.ToAllPackageAssemblies();
                    y.Applies.ToThisAssembly();
                    y.Applies.ToAssemblyContainingType(registry.GetType());

                    y.AddAllTypesOf<IBuildMenuTree>();
                });

                x.SetServiceIfNone<IBuildMenu, DefaultMenuBuilder>();
            });
        }
    }
}