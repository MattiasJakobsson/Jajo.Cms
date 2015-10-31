using System.Collections.Generic;
using Jajo.Cms.Templates;

namespace Jajo.Cms.Rendering
{
    public class TemplateRenderInformation : IRenderInformation
    {
        public TemplateRenderInformation(IEnumerable<IRequestContext> contexts, CmsTemplate template, IDictionary<string, object> overrideSettings = null)
        {
            Contexts = contexts;
            Template = template;
            OverrideSettings = overrideSettings ?? new Dictionary<string, object>();
        }

        public IEnumerable<IRequestContext> Contexts { get; private set; }
        public CmsTemplate Template { get; private set; }
        public IDictionary<string, object> OverrideSettings { get; private set; }
    }
}