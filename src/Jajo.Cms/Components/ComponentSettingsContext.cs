using System.Collections.Generic;

namespace Jajo.Cms.Components
{
    public class ComponentSettingsContext : IRequestContext
    {
        public ComponentSettingsContext(IDictionary<string, object> settings)
        {
            Settings = settings;
        }

        public IDictionary<string, object> Settings { get; private set; }
    }
}