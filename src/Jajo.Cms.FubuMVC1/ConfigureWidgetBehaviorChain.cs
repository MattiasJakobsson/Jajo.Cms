using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace Jajo.Cms.FubuMVC1
{
    public class ConfigureWidgetBehaviorChain : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            foreach (var behavior in graph.Behaviors)
            {
                var input = behavior.InputType();

                if (input != null)
                {
                    if (typeof(ISupplyContext).IsAssignableFrom(input))
                        behavior.Prepend(new Wrapper(typeof(SetContexts<>).MakeGenericType(input)));
                }
            }

            foreach (var action in graph.Actions())
            {
                if (action.HasOutput && typeof(ISupplyContext).IsAssignableFrom(action.OutputType()))
                {
                    action.AddAfter(new Wrapper(typeof(SetContexts<>).MakeGenericType(action.OutputType())));
                }
            }
        }
    }
}