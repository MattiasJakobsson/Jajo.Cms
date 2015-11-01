using FubuMVC.Core;

namespace Jajo.Cms.FubuMVC1.Features
{
    public class FeatureRegistryExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Add<ConfigureFeatureBehaviorChain>();
        }
    }
}