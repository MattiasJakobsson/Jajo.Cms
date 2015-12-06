using System;
using System.Collections.Generic;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Parsing
{
    public interface ITextParser
    {
        string Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme, Func<string, string> recurse);
        IEnumerable<string> GetTags();
    }
}