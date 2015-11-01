using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace Jajo.Cms.FubuMVC1.Features
{
    public class ConfigureFeatureBehaviorChain : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            foreach (var behavior in graph.Behaviors)
            {
                if (behavior.InputType() != null)
                    behavior.Prepend(new Wrapper(typeof(EnsureFeatureIsEnabled<>).MakeGenericType(behavior.InputType())));
            }
        }
    }
}