using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FubuCore;
using FubuMVC.Core.Runtime;
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
        private readonly IOutputWriter _outputWriter;

        public FubuSparkCmsViewEngine(ISparkTemplateRegistry sparkTemplateRegistry, IServiceLocator services, IViewEntryProviderCache viewEntryProviderCache, IOutputWriter outputWriter)
        {
            _sparkTemplateRegistry = sparkTemplateRegistry;
            _services = services;
            _viewEntryProviderCache = viewEntryProviderCache;
            _outputWriter = outputWriter;
        }

        public CmsView FindView<TModel>(string viewName, TModel model, ITheme theme, IEnumerable<IRequestContext> contexts, bool useMaster) where TModel : class
        {
            var sparkViewEntry = BuildViewEntry(model, useMaster);

            if (sparkViewEntry == null)
                return null;

            return new CmsView((renderTo, contentType) =>
            {
                var view = sparkViewEntry.CreateInstance();

                SetModel(model, view);

                var result = _outputWriter.Record(() => _outputWriter.Write(contentType, x =>
                {
                    var writer = new StreamWriter(x);

                    view.RenderView(writer);

                    writer.Flush();
                }));

                return renderTo.WriteAsync(result.GetText());
            });
        }

        public ISparkViewEntry BuildViewEntry(object model, bool useMaster)
        {
            var descriptor = _sparkTemplateRegistry
                .ViewDescriptors()
                .FirstOrDefault(x => x.ViewModel == model.GetType());

            if (descriptor == null)
                return null;

            var sparkViewDescriptor = useMaster ? descriptor.ToSparkViewDescriptor() : descriptor.ToPartialSparkViewDescriptor();

            return _viewEntryProviderCache.GetViewEntry(sparkViewDescriptor);
        }

        private void SetModel<TModel>(TModel model, ISparkView view) where TModel : class
        {
            var fubuPage = view as FubuSparkView<TModel>;

            if (fubuPage == null)
                return;

            fubuPage.ServiceLocator = _services;
            fubuPage.SetModel(model);
        }
    }
}