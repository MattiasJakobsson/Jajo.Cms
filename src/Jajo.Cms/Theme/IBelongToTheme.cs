using System;

namespace Jajo.Cms.Theme
{
    public interface IBelongToTheme
    {
        Type GetTheme();
    }
}