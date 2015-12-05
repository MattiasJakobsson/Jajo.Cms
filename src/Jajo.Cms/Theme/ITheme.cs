using System.Collections.Generic;

namespace Jajo.Cms.Theme
{
    public interface ITheme
    {
        string GetName();
        string GetCategory();

        IDictionary<string, object> GetDefaultSettings();
    }
}