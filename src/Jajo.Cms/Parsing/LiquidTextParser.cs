using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DotLiquid;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Parsing
{
    public class LiquidTextParser : ITextParser
    {
        private static readonly Cache<string, Template> ParsedTemplates = new Cache<string, Template>();

        public string Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme, Func<string, string> recurse)
        {
            var hash = CalculateHash(text);

            var template = ParsedTemplates.Get(hash, x => Template.Parse(text));

            var contexts = context
                .FindCurrentContexts()
                .ToDictionary(x => x.Name, x => (object)x.Data);

            return template.Render(Hash.FromDictionary(contexts));
        }

        public IEnumerable<string> GetTags()
        {
            yield return "tohtml";
            yield return "singletarget";
        }

        private static string CalculateHash(string input)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var t in hash)
                sb.Append(t.ToString("X2"));

            return sb.ToString();
        }
    }
}