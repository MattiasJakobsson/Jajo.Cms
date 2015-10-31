using System;
using System.Collections.Generic;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Templates
{
    public class CmsTemplate : IRequireContexts, IBelongToTheme
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public string Category { get; set; }
        public IEnumerable<Type> RequiredContexts { get; set; }
        public Type ForTheme { get; set; }
        public IDictionary<string, object> Settings { get; set; }
        
        public IEnumerable<Type> GetRequiredContexts()
        {
            return RequiredContexts;
        }

        public Type GetTheme()
        {
            return ForTheme;
        }
    }
}