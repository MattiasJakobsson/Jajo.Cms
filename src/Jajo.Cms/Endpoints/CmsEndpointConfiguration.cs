using System.Collections.Generic;

namespace Jajo.Cms.Endpoints
{
    public class CmsEndpointConfiguration
    {
        public string EndpointName { get; set; }
        public string Theme { get; set; }
        public IDictionary<string, object> Settings { get; set; }
    }
}