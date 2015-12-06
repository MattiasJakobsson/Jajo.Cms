using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Parsing
{
    public class ContextParametersTextParser : RegexTextParser
    {
        private readonly IFindParameterValueFromModel _findParameterValueFromModel;

        public ContextParametersTextParser(IFindParameterValueFromModel findParameterValueFromModel)
        {
            _findParameterValueFromModel = findParameterValueFromModel;
        }

        public override IEnumerable<string> GetTags()
        {
            yield return "parameter";
            yield return "multitarget";
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme, Func<string, string> recurse)
        {
            var contextNameGroup = match.Groups["contextName"];

            if (contextNameGroup == null || string.IsNullOrEmpty(contextNameGroup.Value))
                return "";

            var contextName = match.Groups["contextName"].Value;

            var contextType = contextName.Split('.').FirstOrDefault();

            var requestContext = context.FindContext(contextType);

            if (requestContext == null)
                return null;

            var value = _findParameterValueFromModel.Find(contextName.Substring(string.Format("{0}.", contextType).Length), requestContext);

            var stringValue = value as string;

            return stringValue != null ? recurse(stringValue) : value;
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\$\{Contexts\.((?<contextName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*))\}", RegexOptions.Compiled);
        }
    }
}