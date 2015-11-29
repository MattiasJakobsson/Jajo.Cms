using Jajo.Cms.Theme;

namespace Jajo.Cms.Endpoints
{
    public interface IEndpointConfigurationStorage
    {
        CmsEndpointConfiguration Load(string endpoint, ITheme theme);
    }
}