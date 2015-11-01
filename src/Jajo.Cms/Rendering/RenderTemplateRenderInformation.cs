using System.IO;
using System.Threading.Tasks;
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

        public async Task Render(TemplateRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            var renderResult = await _cmsRenderer.RenderTemplate(information.Template, information.OverrideSettings, context, theme);
            await renderResult.RenderTo(renderTo);
        }
    }
}