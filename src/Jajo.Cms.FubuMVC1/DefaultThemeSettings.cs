using System.Collections.Generic;
using System.Linq;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1
{
    public class DefaultThemeSettings : IThemeSettings
    {
        private readonly IEnumerable<ITheme> _themes;

        public DefaultThemeSettings(IEnumerable<ITheme> themes)
        {
            _themes = themes;
        }

        public ITheme GetCurrentTheme()
        {
            return _themes.FirstOrDefault();
        }
    }
}