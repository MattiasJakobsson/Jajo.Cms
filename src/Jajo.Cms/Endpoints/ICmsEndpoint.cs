using System.Collections.Generic;
using Jajo.Cms.Rendering;

namespace Jajo.Cms.Endpoints
{
    public interface ICmsEndpoint
    {
        string GetName();
        IDictionary<string, object> GetDefaultSettings();
    }

    public interface ICmsEndpoint<in TInput> : ICmsEndpoint
        where TInput : ICmsEndpointInput
    {
        IRenderInformation Render(TInput input, ICmsContext context, IDictionary<string, object> settings);
    }
}