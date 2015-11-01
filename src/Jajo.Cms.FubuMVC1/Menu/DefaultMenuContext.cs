using FubuCore;
using FubuMVC.Core.Runtime;
using Jajo.Cms.Menu;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class DefaultMenuContext : IMenuContext
    {
        private readonly IFubuRequest _fubuRequest;
        private readonly IServiceLocator _serviceLocator;

        public DefaultMenuContext(IFubuRequest fubuRequest, IServiceLocator serviceLocator)
        {
            _fubuRequest = fubuRequest;
            _serviceLocator = serviceLocator;
        }

        public T Get<T>() where T : class
        {
            return _fubuRequest.Get<T>();
        }

        public TService Service<TService>()
        {
            return _serviceLocator.GetInstance<TService>();
        }
    }
}