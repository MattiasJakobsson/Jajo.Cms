using System.IO;
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

        public void Render(TextRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            var result = _cmsRenderer.ParseText(information.Text, context, theme);
            result.RenderTo(renderTo);
        }
    }
}