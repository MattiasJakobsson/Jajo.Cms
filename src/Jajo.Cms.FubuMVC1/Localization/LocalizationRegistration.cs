using FubuMVC.Core;
using FubuMVC.Core.Registration;
using Jajo.Cms.Localization;

namespace Jajo.Cms.FubuMVC1.Localization
{
    public class LocalizationRegistration : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(x =>
            {
                x.SetServiceIfNone<ILocalizeText, DefaultTextLocalizer>();
                x.SetServiceIfNone<IFindCurrentLocalizationNamespace, DefaultLocalizationNamespaceFinder>();

                x.Scan(y =>
                {
                    y.Applies.ToAllPackageAssemblies();
                    y.Applies.ToThisAssembly();
                    y.Applies.ToAssemblyContainingType(registry.GetType());
                    y.Applies.ToAssemblyContainingType<ILocalizationVisitor>();

                    y.AddAllTypesOf<ILocalizationVisitor>();
                });
            });
        }
    }
}