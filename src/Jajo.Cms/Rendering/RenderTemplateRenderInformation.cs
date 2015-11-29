using System.IO;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Rendering
{
    public class RenderTemplateRenderInformation : ICmsRenderer<TemplateRenderInformation>
    {
        private readonly ICmsRenderer _cmsRenderer;

        public RenderTemplateRenderInformation(ICmsRenderer cmsRenderer)
        {
            _cmsRenderer = cmsRenderer;
        }

        public void Render(TemplateRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            var renderResult = _cmsRenderer.RenderTemplate(information.Template, information.OverrideSettings, context, theme);
            renderResult.RenderTo(renderTo);
        }
    }
}