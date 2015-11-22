using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Views
{
    public class RenderViewRenderInformation : ICmsRenderer<ViewRenderInformation>
    {
        private readonly IEnumerable<ICmsViewEngine> _viewEngines;

        public RenderViewRenderInformation(IEnumerable<ICmsViewEngine> viewEngines)
        {
            _viewEngines = viewEngines;
        }

        public Task Render(ViewRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            var view = _viewEngines
                .Select(x => FindViewFrom(x, information.ViewName, information.Model, theme, information.Contexts))
                .FirstOrDefault(x => x != null);

            if (view == null)
                return Task.CompletedTask;

            return view.Render(renderTo);
        }

        private static CmsView FindViewFrom(ICmsViewEngine viewEngine, string viewName, object model, ITheme theme, IEnumerable<IRequestContext> contexts)
        {
            var findViewMethod = viewEngine
                .GetType()
                .GetMethod("FindView");

            var genericFindViewMethod = findViewMethod.MakeGenericMethod(model.GetType());

            return (CmsView)genericFindViewMethod.Invoke(viewEngine, new[] { viewName, model, theme, contexts });
        }
    }
}