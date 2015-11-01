using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Jajo.Cms.Components;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Templates;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Rendering
{
    public interface ICmsRenderer
    {
        Task<IRenderResult> RenderEndpoint(ICmsEndpointInput input, ICmsContext context, ITheme theme);
        Task<IRenderResult> RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context, ITheme theme);
        Task<IRenderResult> RenderTemplate(CmsTemplate template, IDictionary<string, object> settings, ICmsContext context, ITheme theme);
        Task<IRenderResult> ParseText(string text, ICmsContext context, ITheme theme);
    }

    public interface ICmsRenderer<in TRenderInformation> where TRenderInformation : IRenderInformation
    {
        Task Render(TRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo);
    }
}