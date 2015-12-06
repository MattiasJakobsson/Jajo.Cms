using Jajo.Cms.Localization;

namespace Jajo.Cms.Tests
{
    public class FakeNamespaceFinder : IFindCurrentLocalizationNamespace
    {
        public string Find()
        {
            return "";
        }
    }
}