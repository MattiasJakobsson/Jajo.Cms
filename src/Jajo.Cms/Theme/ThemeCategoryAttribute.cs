using System;

namespace Jajo.Cms.Theme
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ThemeCategoryAttribute : Attribute
    {
        public ThemeCategoryAttribute(string category)
        {
            Category = category;
        }

        public string Category { get; private set; } 
    }
}