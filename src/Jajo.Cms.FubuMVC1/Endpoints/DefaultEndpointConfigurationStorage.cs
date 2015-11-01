using System.Collections.Generic;
using System.Threading.Tasks;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1.Endpoints
{
    public class DefaultEndpointConfigurationStorage : IEndpointConfigurationStorage
    {
        public Task<CmsEndpointConfiguration> Load(string endpoint, ITheme theme)
        {
            return Task.FromResult(new CmsEndpointConfiguration
            {
                Settings = new Dictionary<string, object>(),
                EndpointName = endpoint,
                Theme = theme.GetName()
            });
        }
    }
}