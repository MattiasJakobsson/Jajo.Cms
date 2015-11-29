using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Jajo.Cms.Components;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;
using Newtonsoft.Json;

namespace Jajo.Cms.Parsing
{
    public class ComponentTextParser : RegexTextParser
    {
        private readonly IEnumerable<ICmsComponent> _components;

        public ComponentTextParser(IEnumerable<ICmsComponent> components)
        {
            _components = components;
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, ITheme theme)
        {
            var componentNameGroup = match.Groups["componentName"];

            if (componentNameGroup == null)
                return "";

            var componentName = componentNameGroup.Value;

            var component = context.Filter(_components.Where(x => x.GetType().Name == componentName), theme).FirstOrDefault();

            if (component == null)
                return "";

            var settings = component.GetDefaultSettings();

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

            var renderResult = cmsRenderer.RenderComponent(component, settings, context, theme);
            renderResult.RenderTo(writer);

            writer.Flush();
            stream.Position = 0;

            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\$\{Components\.((?<componentName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*))\}\$", RegexOptions.Compiled);
            yield return new Regex(@"\$\{Components\.((?<componentName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*)) Settings\=((?<settings>[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[\{\}]*))\}\$", RegexOptions.Compiled);
        }
    }
}