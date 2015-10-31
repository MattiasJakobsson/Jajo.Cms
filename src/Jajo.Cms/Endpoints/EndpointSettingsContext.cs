using System.Collections.Generic;

namespace Jajo.Cms.Endpoints
{
    public class EndpointSettingsContext : IRequestContext
    {
        public EndpointSettingsContext(IDictionary<string, object> settings)
        {
            Settings = settings;
        }

        public IDictionary<string, object> Settings { get; private set; } 
    }
}