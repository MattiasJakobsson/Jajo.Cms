using System.IO;
using System.Threading.Tasks;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Rendering
{
    public class RenderTextRenderInformation : ICmsRenderer<TextRenderInformation>
    {
        private readonly ICmsRenderer _cmsRenderer;

        public RenderTextRenderInformation(ICmsRenderer cmsRenderer)
        {
            _cmsRenderer = cmsRenderer;
        }

        public Task Render(TextRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            return _cmsRenderer.ParseText(information.Text, context, theme, renderTo);
        }
    }
}