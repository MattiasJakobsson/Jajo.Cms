using System.Collections.Generic;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Tests
{
    public class FakeTheme : ITheme
    {
        public string GetName()
        {
            return "faketheme";
        }

        public string GetCategory()
        {
            return "";
        }

        public IDictionary<string, object> GetDefaultSettings()
        {
            return new Dictionary<string, object>();
        }

        public bool IsTranslationKeyForTheme(string key)
        {
            return true;
        }
    }
}