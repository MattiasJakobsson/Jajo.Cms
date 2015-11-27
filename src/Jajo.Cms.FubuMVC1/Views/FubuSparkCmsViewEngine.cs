using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FubuCore;
using FubuMVC.Core.Caching;
using FubuMVC.Core.Http.Headers;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI;
using FubuMVC.Core.Urls;
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
        private readonly ISparkTemplateRegistry _sparkTemplateRegistry;
        private readonly IServiceLocator _services;
        private readonly IViewEntryProviderCache _viewEntryProviderCache;
        private readonly IPartialInvoker _partialInvoker;

        public FubuSparkCmsViewEngine(ISparkTemplateRegistry sparkTemplateRegistry, IServiceLocator services, IViewEntryProviderCache viewEntryProviderCache, IPartialInvoker partialInvoker)
        {
            _sparkTemplateRegistry = sparkTemplateRegistry;
            _services = services;
            _viewEntryProviderCache = viewEntryProviderCache;
            _partialInvoker = partialInvoker;
        }

        public CmsView FindView<TModel>(string viewName, TModel model, ITheme theme, IEnumerable<IRequestContext> contexts, bool useMaster) where TModel : class
        {
            return new CmsView((x, y) =>
            {
                if (useMaster)
                {
                    var sparkViewEntry = BuildViewEntry(model);

                    if (sparkViewEntry == null)
                        return Task.CompletedTask;

                    var view = sparkViewEntry.CreateInstance();

                    SetModel(model, view);

                    view.RenderView(x);

                    return Task.CompletedTask;
                }

                var result = _partialInvoker.InvokeObject(model);

                return x.WriteAsync(result);
            });
        }

        public ISparkViewEntry BuildViewEntry(object model)
        {
            var descriptor = _sparkTemplateRegistry
                .ViewDescriptors()
                .FirstOrDefault(x => x.ViewModel == model.GetType());

            if (descriptor == null)
                return null;

            var sparkViewDescriptor = descriptor.ToSparkViewDescriptor();

            return _viewEntryProviderCache.GetViewEntry(sparkViewDescriptor);
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