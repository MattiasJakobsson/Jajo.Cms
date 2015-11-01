using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1
{
    public interface IThemeSettings
    {
        ITheme GetCurrentTheme();
    }
}