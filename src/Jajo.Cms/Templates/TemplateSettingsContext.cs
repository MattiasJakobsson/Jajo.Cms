using System.Collections.Generic;

namespace Jajo.Cms.Templates
{
    public class TemplateSettingsContext : IRequestContext
    {
        public TemplateSettingsContext(IDictionary<string, object> settings)
        {
            Settings = settings;
        }

        public IDictionary<string, object> Settings { get; private set; }
    }
}