using System;
using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;

namespace Jajo.Cms.FubuMVC1
{
    public class SetContexts<TContextSupplier> : IActionBehavior where TContextSupplier : class, ISupplyContext
    {
        private readonly IActionBehavior _innerBehavior;
        private readonly IFubuRequest _fubuRequest;
        private readonly ICmsContext _cmsContext;

        public SetContexts(IActionBehavior innerBehavior, IFubuRequest fubuRequest, ICmsContext cmsContext)
        {
            _innerBehavior = innerBehavior;
            _fubuRequest = fubuRequest;
            _cmsContext = cmsContext;
        }

        public void Invoke()
        {
            var input = _fubuRequest.Get<TContextSupplier>();
            var contexts = input.GetContexts().ToDictionary(x => Guid.NewGuid(), x => x);

            foreach (var context in contexts)
                _cmsContext.EnterContext(context.Key, context.Value);

            _innerBehavior?.Invoke();

            foreach (var context in contexts)
                _cmsContext.ExitContext(context.Key);
        }

        public void InvokePartial()
        {
            var input = _fubuRequest.Get<TContextSupplier>();
            var contexts = input.GetContexts().ToDictionary(x => Guid.NewGuid(), x => x);

            foreach (var context in contexts)
                _cmsContext.EnterContext(context.Key, context.Value);

            _innerBehavior?.InvokePartial();

            foreach (var context in contexts)
                _cmsContext.ExitContext(context.Key);
        }
    }
}