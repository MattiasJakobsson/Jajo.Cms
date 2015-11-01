using System.Collections.Generic;
using System.Threading.Tasks;
using Jajo.Cms.Rendering;

namespace Jajo.Cms.Components
{
    public interface ICmsComponent
    {
        string Name { get; }
        string Category { get; }

        Task<IRenderInformation> Render(ICmsContext context, IDictionary<string, object> settings);
        IDictionary<string, object> GetDefaultSettings();
    }
}