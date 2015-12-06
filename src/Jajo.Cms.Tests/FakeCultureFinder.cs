using System.Globalization;
using UStack.Infrastructure;

namespace Jajo.Cms.Tests
{
    public class FakeCultureFinder : IFindCultureForRequest
    {
        public CultureInfo FindCulture()
        {
            return new CultureInfo("sv-SE");
        }

        public CultureInfo FindUiCulture()
        {
            return new CultureInfo("sv-SE");
        }
    }
}