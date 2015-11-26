using System.Collections.Generic;
using System.IO;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View;
using Jajo.Cms.Components;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1
{
    public static class CmsFubuPageExtensions
    {
        public static void Component(this IFubuPage page, ICmsComponent component, IDictionary<string, object> settings = null, ITheme theme = null)
        {
            var cmsRenderer = page.ServiceLocator.GetInstance<ICmsRenderer>();
            var cmsContext = page.ServiceLocator.GetInstance<ICmsContext>();
            var outputWriter = page.ServiceLocator.GetInstance<IOutputWriter>();

            var result = cmsRenderer.RenderComponent(component, settings ?? new Dictionary<string, object>(), page.ServiceLocator.GetInstance<ICmsContext>(), theme ?? cmsContext.GetCurrentTheme()).Result;

            outputWriter.Write(result.ContentType, x =>
            {
                var writer = new StreamWriter(x);

                result.RenderTo(writer).Wait();

                writer.Flush();
            });
        }
    }
}