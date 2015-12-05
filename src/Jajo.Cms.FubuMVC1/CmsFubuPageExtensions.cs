using System.Collections.Generic;
using System.IO;
using System.Web;
using FubuMVC.Core.View;
using Jajo.Cms.Components;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1
{
    public static class CmsFubuPageExtensions
    {
        public static IHtmlString Component(this IFubuPage page, ICmsComponent component, IDictionary<string, object> settings = null, ITheme theme = null)
        {
            var cmsRenderer = page.ServiceLocator.GetInstance<ICmsRenderer>();
            var cmsContext = page.ServiceLocator.GetInstance<ICmsContext>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            var result = cmsRenderer.RenderComponent(component, settings ?? new Dictionary<string, object>(), cmsContext, theme ?? cmsContext.GetCurrentTheme());

            result.RenderTo(writer);
            writer.Flush();
            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                return new HtmlString(reader.ReadToEnd());
            }
        }

        public static IHtmlString ParseText(this IFubuPage page, string text, ITheme theme = null)
        {
            var cmsRenderer = page.ServiceLocator.GetInstance<ICmsRenderer>();
            var cmsContext = page.ServiceLocator.GetInstance<ICmsContext>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            var result = cmsRenderer.ParseText(text, cmsContext, theme ?? cmsContext.GetCurrentTheme());

            result.RenderTo(writer);
            writer.Flush();
            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                return new HtmlString(reader.ReadToEnd());
            }
        }
    }
}