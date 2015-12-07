using System;
using System.Collections.Generic;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Templates
{
    public class CmsTemplate : IRequireContexts, IBelongToTheme
    {
        private readonly Type _forTheme;
        private readonly IEnumerable<string> _requiredContexts;

        public CmsTemplate(string name, string body, string contentType, Type forTheme, params string[] requiredContexts)
        {
            _forTheme = forTheme;
            _requiredContexts = requiredContexts;
            Name = name;
            Body = body;
            ContentType = contentType;
        }

        public string Name { get; private set; }
        public string Body { get; private set; }
        public string ContentType { get; private set; }
        
        public IEnumerable<string> GetRequiredContexts()
        {
            return _requiredContexts;
        }

        public Type GetTheme()
        {
            return _forTheme;
        }
    }
}