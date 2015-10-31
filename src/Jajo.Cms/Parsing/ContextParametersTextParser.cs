using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        protected override Task<object> FindParameterValue(Match match, ICmsContext context, ITheme theme)
        {
            var contextNameGroup = match.Groups["contextName"];

            if (contextNameGroup == null || string.IsNullOrEmpty(contextNameGroup.Value))
                return Task.FromResult<object>("");

            var contextName = match.Groups["contextName"].Value;

            var contextType = contextName.Split('.').FirstOrDefault();

            var requestContext = context.FindContext(contextType);

            if (requestContext == null)
                return null;

            return Task.FromResult(_findParameterValueFromModel.Find(contextName.Substring(string.Format("{0}.", contextType).Length), requestContext));
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\$\{Contexts\.((?<contextName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*))\}", RegexOptions.Compiled);
        }
    }
}