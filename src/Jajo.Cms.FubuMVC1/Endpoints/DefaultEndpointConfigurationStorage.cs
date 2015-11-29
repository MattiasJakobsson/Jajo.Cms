using System.Collections.Generic;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1.Endpoints
{
    public class DefaultEndpointConfigurationStorage : IEndpointConfigurationStorage
    {
        public CmsEndpointConfiguration Load(string endpoint, ITheme theme)
        {
            return new CmsEndpointConfiguration
            {
                Settings = new Dictionary<string, object>(),
                EndpointName = endpoint,
                Theme = theme.GetName()
            };
        }
    }
}