using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;
using Jajo.Cms.Features;
using UStack.Infrastructure.Web.EndpointValidation;

namespace Jajo.Cms.FubuMVC1.Features
{
    public class EnsureFeatureIsEnabled<TInput> : IActionBehavior where TInput : class, IBelongToFeatures
    {
        private readonly IFubuRequest _fubuRequest;
        private readonly IActionBehavior _innerBehavior;
        private readonly ICurrentHttpRequest _currentHttpRequest;
        private readonly IHandle404 _handle404;
        private readonly IFeatureValidator _featureValidator;

        public EnsureFeatureIsEnabled(IFubuRequest fubuRequest, IActionBehavior innerBehavior, ICurrentHttpRequest currentHttpRequest, IHandle404 handle404, IFeatureValidator featureValidator)
        {
            _fubuRequest = fubuRequest;
            _innerBehavior = innerBehavior;
            _currentHttpRequest = currentHttpRequest;
            _handle404 = handle404;
            _featureValidator = featureValidator;
        }

        public void Invoke()
        {
            var features = _fubuRequest.Get<TInput>().GetFeatures();

            if (features.Any(x => !_featureValidator.IsActive(x)))
            {
                _handle404.Handle(_currentHttpRequest.FullUrl());

                return;
            }

            if (_innerBehavior != null)
                _innerBehavior.Invoke();
        }

        public void InvokePartial()
        {
            var features = _fubuRequest.Get<TInput>().GetFeatures();

            if (features.Any(x => !_featureValidator.IsActive(x)))
                return;

            if (_innerBehavior != null)
                _innerBehavior.InvokePartial();
        }
    }
}