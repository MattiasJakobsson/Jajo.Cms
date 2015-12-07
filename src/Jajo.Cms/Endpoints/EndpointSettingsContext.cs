using System.Collections.Generic;

namespace Jajo.Cms.Endpoints
{
    public class EndpointSettingsContext
    {
        public static RequestContext Build(IDictionary<string, object> settings)
        {
            return new RequestContext(typeof(EndpointSettingsContext).Name, new Dictionary<string, object>
            {
                {"Settings", settings}
            });
        }
    }
}