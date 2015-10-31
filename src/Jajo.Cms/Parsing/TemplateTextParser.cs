using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jajo.Cms.Rendering;
using Jajo.Cms.Templates;
using Jajo.Cms.Theme;
using Newtonsoft.Json;

namespace Jajo.Cms.Parsing
{
    public class TemplateTextParser : RegexTextParser
    {
        private readonly ICmsRenderer _cmsRenderer;
        private readonly ITemplateStorage _templateStorage;

        public TemplateTextParser(ICmsRenderer cmsRenderer, ITemplateStorage templateStorage)
        {
            _cmsRenderer = cmsRenderer;
            _templateStorage = templateStorage;
        }

        protected override async Task<object> FindParameterValue(Match match, ICmsContext context, ITheme theme)
        {
            var templateNameGroup = match.Groups["templateName"];

            if (templateNameGroup == null)
                return "";

            var templateName = templateNameGroup.Value;

            var template = _templateStorage.Load(templateName);

            if (template == null)
                return "";

            var settings = new Dictionary<string, object>();

            var settingsGroup = match.Groups["settings"];

            if (settingsGroup != null && !string.IsNullOrEmpty(settingsGroup.Value))
            {
                var settingsJson = settingsGroup.Value.Substring("settings=".Length);

                if (!string.IsNullOrEmpty(settingsJson))
                {
                    var parsedSettings = JsonConvert.DeserializeObject<IDictionary<string, object>>(settingsJson);

                    foreach (var item in parsedSettings)
                        settings[item.Key] = item.Value;
                }
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            await _cmsRenderer.RenderTemplate(template, settings, context, theme, writer);

            await writer.FlushAsync();
            stream.Position = 0;

            using (var reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\$\{Templates\.((?<templateName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*))\}\$", RegexOptions.Compiled);
            yield return new Regex(@"\$\{Templates\.((?<templateName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*)) Settings\=((?<settings>[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[\{\}]*))\}\$", RegexOptions.Compiled);
        }
    }
}