using System.IO;
using Jajo.Cms.Theme;
using Newtonsoft.Json;

namespace Jajo.Cms.Rendering
{
    public class RenderJsonRenderInformation : ICmsRenderer<JsonRenderInformation>
    {
        public void Render(JsonRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            var json = JsonConvert.SerializeObject(information.Data);
            renderTo.Write(json);
        }
    }
}