using FubuMVC.Core;
using Jajo.Cms.Features;

namespace Jajo.Cms.FubuMVC1.Features
{
    public class FeatureRegistryExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(x => x.SetServiceIfNone<IFeatureValidator, DefaultFeatureValidator>());

            registry.Policies.Add<ConfigureFeatureBehaviorChain>();
        }
    }
}