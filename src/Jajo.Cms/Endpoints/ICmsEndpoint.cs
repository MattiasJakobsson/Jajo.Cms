using System.Collections.Generic;
using Jajo.Cms.Rendering;

namespace Jajo.Cms.Endpoints
{
    public interface ICmsEndpoint
    {
        string GetName();
        IRenderInformation Render(ICmsContext context, IDictionary<string, object> settings);
        IDictionary<string, object> GetDefaultSettings();
    }

    public interface ICmsEndpoint<TInput> : ICmsEndpoint
        where TInput : ICmsEndpointInput
    {
         
    }
}