using System.Collections.Generic;

namespace Jajo.Cms.Rendering
{
    public class JsonRenderInformation : IRenderInformation
    {
        public JsonRenderInformation(IEnumerable<IRequestContext> contexts, object data)
        {
            Contexts = contexts;
            Data = data;
        }

        public string ContentType { get { return "Application/Json"; } }
        public object Data { get; private set; }
        public IEnumerable<IRequestContext> Contexts { get; private set; }
    }
}