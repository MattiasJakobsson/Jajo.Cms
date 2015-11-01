using System.Threading.Tasks;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Endpoints
{
    public interface IEndpointConfigurationStorage
    {
        Task<CmsEndpointConfiguration> Load(string endpoint, ITheme theme);
    }
}