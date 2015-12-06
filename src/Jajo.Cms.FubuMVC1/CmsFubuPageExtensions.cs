using System.Collections.Generic;
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

            var result = cmsRenderer.RenderComponent(component, settings ?? new Dictionary<string, object>(), cmsContext, theme ?? cmsContext.GetCurrentTheme());

            return new HtmlString(result.Read());
        }

        public static IHtmlString ParseText(this IFubuPage page, string text, ITheme theme = null)
        {
            var cmsRenderer = page.ServiceLocator.GetInstance<ICmsRenderer>();
            var cmsContext = page.ServiceLocator.GetInstance<ICmsContext>();

            var result = cmsRenderer.ParseText(text, cmsContext, theme ?? cmsContext.GetCurrentTheme());

            return new HtmlString(result.Read());
        }
    }
}