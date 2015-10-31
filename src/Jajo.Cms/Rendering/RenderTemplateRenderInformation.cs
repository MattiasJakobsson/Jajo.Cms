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

        public Task Render(TemplateRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            return _cmsRenderer.RenderTemplate(information.Template, information.OverrideSettings, context, theme, renderTo);
        }
    }
}