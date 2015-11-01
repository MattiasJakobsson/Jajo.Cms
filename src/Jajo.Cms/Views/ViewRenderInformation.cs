using System.Collections.Generic;
using Jajo.Cms.Rendering;

namespace Jajo.Cms.Views
{
    public class ViewRenderInformation : IRenderInformation
    {
        public ViewRenderInformation(string viewName, object model, IEnumerable<IRequestContext> contexts, string contentType)
        {
            ViewName = viewName;
            Model = model;
            Contexts = contexts;
            ContentType = contentType;
        }

        public string ViewName { get; private set; }
        public object Model { get; private set; }
        public string ContentType { get; private set; }
        public IEnumerable<IRequestContext> Contexts { get; private set; }
    }
}