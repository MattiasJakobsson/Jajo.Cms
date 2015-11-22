using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FubuCore;
using FubuMVC.Core.Http;
using FubuMVC.Spark.Registration;
using FubuMVC.Spark.Rendering;
using FubuMVC.Spark.SparkModel;
using Jajo.Cms.Theme;
using Jajo.Cms.Views;
using Spark;

namespace Jajo.Cms.FubuMVC1.Views
{
    public class FubuSparkCmsViewEngine : ICmsViewEngine
    {
        private readonly IRequestHeaders _headers;
        private readonly ICurrentChain _chains;
        private readonly ISparkTemplateRegistry _sparkTemplateRegistry;
        private readonly ISparkViewEngine _viewEngine;
        private readonly IServiceLocator _services;

        public FubuSparkCmsViewEngine(IRequestHeaders headers, ICurrentChain chains, ISparkTemplateRegistry sparkTemplateRegistry, ISparkViewEngine viewEngine, IServiceLocator services)
        {
            _headers = headers;
            _chains = chains;
            _sparkTemplateRegistry = sparkTemplateRegistry;
            _viewEngine = viewEngine;
            _services = services;
        }

        public CmsView FindView<TModel>(string viewName, TModel model, ITheme theme, IEnumerable<IRequestContext> contexts) where TModel : class
        {
            var sparkViewEntry = BuildViewEntry(model);

            if (sparkViewEntry == null)
                return null;

            return new CmsView(x =>
            {
                var view = sparkViewEntry.CreateInstance();

                SetModel(model, view);

                view.RenderView(x);

                return Task.CompletedTask;
            });
        }

        public ISparkViewEntry BuildViewEntry(object model)
        {
            var descriptor = _sparkTemplateRegistry
                .ViewDescriptors()
                .FirstOrDefault(x => x.ViewModel == model.GetType());

            if (descriptor == null)
                return null;

            if (_chains.IsInPartial() || _headers.IsAjaxRequest())
                descriptor.Master = null;

            var sparkViewDescriptor = descriptor.ToSparkViewDescriptor();

            return _viewEngine.CreateEntry(sparkViewDescriptor);
        }

        private void SetModel<TModel>(TModel model, ISparkView view) where TModel : class
        {
            var fubuPage = view as FubuSparkView<TModel>;

            if(fubuPage == null)
                return;

            fubuPage.ServiceLocator = _services;
            fubuPage.SetModel(model);
        }
    }
}