using System.Collections.Generic;
using System.IO;
using System.Web;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration.Nodes;
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
            var currentChain = page.ServiceLocator.GetInstance<ICurrentChain>();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            currentChain.Push(new BehaviorChain());
            var result = cmsRenderer.RenderComponent(component, settings ?? new Dictionary<string, object>(), page.ServiceLocator.GetInstance<ICmsContext>(), theme ?? cmsContext.GetCurrentTheme()).Result;

            result.RenderTo(writer).Wait();
            writer.Flush();
            stream.Position = 0;

            currentChain.Pop();

            using (var reader = new StreamReader(stream))
            {
                return new HtmlString(reader.ReadToEnd());
            }
        }
    }
}