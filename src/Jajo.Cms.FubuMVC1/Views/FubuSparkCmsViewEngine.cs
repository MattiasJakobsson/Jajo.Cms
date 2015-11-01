using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FubuMVC.Core.Http;
using FubuMVC.Core.View;
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
        private readonly IViewEntryProviderCache _viewEntryProviderCache;
        private readonly IViewDefinitionResolver _viewDefinitionResolver;

        public FubuSparkCmsViewEngine(IRequestHeaders headers, ICurrentChain chains, ISparkTemplateRegistry sparkTemplateRegistry, IViewEntryProviderCache viewEntryProviderCache, 
            IViewDefinitionResolver viewDefinitionResolver)
        {
            _headers = headers;
            _chains = chains;
            _sparkTemplateRegistry = sparkTemplateRegistry;
            _viewEntryProviderCache = viewEntryProviderCache;
            _viewDefinitionResolver = viewDefinitionResolver;
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

            var viewEntrySource = new ViewEntrySource(descriptor, _viewEntryProviderCache, _viewDefinitionResolver);

            return _chains.IsInPartial() || _headers.IsAjaxRequest() ? viewEntrySource.GetViewEntry() : viewEntrySource.GetPartialViewEntry();
        }

        private static void SetModel<TModel>(TModel model, ISparkView view) where TModel : class
        {
            var fubuPage = view as IFubuPage<TModel>;

            if(fubuPage == null)
                return;

            fubuPage.Model = model;
        }
    }
}