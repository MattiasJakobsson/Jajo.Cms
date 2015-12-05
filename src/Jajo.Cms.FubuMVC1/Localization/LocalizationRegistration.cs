using FubuMVC.Core;
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
            });
        }
    }
}