using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Parsing
{
    public abstract class RegexTextParser : ITextParser
    {
        protected virtual string SeperateListItemsWith { get { return "\n"; } }

        public string Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme, Func<string, string> recurse)
        {
            text = text ?? "";

            text = GetRegexes().Aggregate(text, (current, regex) => regex.Replace(current, x =>
            {
                var value = FindParameterValue(x, cmsRenderer, context, theme, recurse);

                if (value == null) return "";

                var enumerableValue = value as IEnumerable;
                if (enumerableValue != null && !(value is string))
                {
                    var stringValues = enumerableValue.OfType<object>().Select(y => y.ToString()).ToList();

                    return string.Join(SeperateListItemsWith, stringValues);
                }

                return value.ToString();
            }));

            return text;
        }

        public abstract IEnumerable<string> GetTags();
        protected abstract object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme, Func<string, string> recurse);
        protected abstract IEnumerable<Regex> GetRegexes();
    }
}