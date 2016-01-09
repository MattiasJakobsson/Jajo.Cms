using System;

namespace Jajo.Cms.Theme
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ThemeAttribute : Attribute
    {
        public ThemeAttribute(Type theme)
        {
            Theme = theme;
        }

        public Type Theme { get; private set; }
    }
}