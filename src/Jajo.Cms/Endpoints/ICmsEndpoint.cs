using System.Collections.Generic;
using System.Threading.Tasks;
using Jajo.Cms.Rendering;

namespace Jajo.Cms.Endpoints
{
    public interface ICmsEndpoint
    {
        string GetName();
        Task<IRenderInformation> Render(ICmsContext context, IDictionary<string, object> settings);
        IDictionary<string, object> GetDefaultSettings();
    }

    public interface ICmsEndpoint<TInput> : ICmsEndpoint
        where TInput : ICmsEndpointInput
    {
         
    }
}