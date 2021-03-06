﻿using System.Collections.Generic;

namespace Jajo.Cms.Theme
{
    public interface ITheme
    {
        string GetName();
        string GetCategory();
        bool IsActive();

        IDictionary<string, object> GetDefaultSettings();
        bool IsTranslationKeyForTheme(string key);
    }
}