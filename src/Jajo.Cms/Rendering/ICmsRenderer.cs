using System.Collections.Generic;
using System.IO;
using Jajo.Cms.Components;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Templates;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Rendering
{
    public interface ICmsRenderer
    {
        IRenderResult RenderEndpoint<TInput>(TInput input, ICmsContext context, ITheme theme) where TInput : ICmsEndpointInput;
        IRenderResult RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context, ITheme theme);
        IRenderResult RenderTemplate(CmsTemplate template, IDictionary<string, object> settings, ICmsContext context, ITheme theme);
        IRenderResult ParseText(string text, ICmsContext context, ITheme theme, ParseTextOptions options = null);
    }

    public interface ICmsRenderer<in TRenderInformation> where TRenderInformation : IRenderInformation
    {
        void Render(TRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo);
    }
}