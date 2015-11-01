using FubuMVC.Core;
using Jajo.Cms.Endpoints;

namespace Jajo.Cms.FubuMVC1.Endpoints
{
    public class ConfigureEndpointBehaviorChain : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(x => x.SetServiceIfNone<IEndpointConfigurationStorage, DefaultEndpointConfigurationStorage>());

            registry.Policies.Add(x =>
            {
                x.Where.ResourceTypeImplements<ICmsEndpointInput>();
                x.ModifyBy(y => y.Output.AddWriter(typeof(CmsEndpointWriter<>).MakeGenericType(y.ResourceType())));
            });
        }
    }
}