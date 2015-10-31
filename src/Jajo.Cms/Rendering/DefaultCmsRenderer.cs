using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jajo.Cms.Components;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Parsing;
using Jajo.Cms.Templates;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Rendering
{
    public class DefaultCmsRenderer : ICmsRenderer
    {
        private readonly IEnumerable<ICmsEndpoint> _themeEndpoints;
        private readonly IEnumerable<ITextParser> _textParsers;
        private readonly IEndpointConfigurationStorage _endpointConfigurationStorage;

        public DefaultCmsRenderer(IEnumerable<ICmsEndpoint> themeEndpoints, IEnumerable<ITextParser> textParsers, IEndpointConfigurationStorage endpointConfigurationStorage)
        {
            _themeEndpoints = themeEndpoints;
            _textParsers = textParsers;
            _endpointConfigurationStorage = endpointConfigurationStorage;
        }

        public async Task RenderEndpoint(ICmsEndpointInput input, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            var themeEndpoint = context
                .Filter(_themeEndpoints.Where(x => typeof(ICmsEndpoint<>).MakeGenericType(input.GetType()).IsInstanceOfType(x)), theme)
                .FirstOrDefault();

            if (themeEndpoint == null)
                return;

            var settings = themeEndpoint.GetDefaultSettings();
            var endpointConfiguration = _endpointConfigurationStorage.Load(theme.GetName(), theme);

            if (endpointConfiguration != null)
            {
                foreach (var item in endpointConfiguration.Settings)
                    settings[item.Key] = item.Value;
            }

            var renderInformation = themeEndpoint.Render(context, settings);

            if (renderInformation == null)
                return;

            var contexts = renderInformation.Contexts.ToDictionary(x => Guid.NewGuid(), x => x);

            foreach (var item in contexts)
                context.EnterContext(item.Key, item.Value);

            var settingsContextId = Guid.NewGuid();
            context.EnterContext(settingsContextId, new EndpointSettingsContext(settings));

            var renderer = (IRenderer)context.Resolve(typeof(Renderer<>).MakeGenericType(renderInformation.GetType()));

            await renderer.Render(renderInformation, context, theme, renderTo);

            context.ExitContext(settingsContextId);

            foreach (var item in contexts)
                context.ExitContext(item.Key);
        }

        public async Task RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            if (!context.CanRender(component, theme))
                return;

            var renderInformation = component.Render(context, settings);

            if (renderInformation == null)
                return;

            var contexts = renderInformation.Contexts.ToDictionary(x => Guid.NewGuid(), x => x);

            foreach (var item in contexts)
                context.EnterContext(item.Key, item.Value);

            var settingsContextId = Guid.NewGuid();
            context.EnterContext(settingsContextId, new ComponentSettingsContext(settings));

            var renderer = (IRenderer)context.Resolve(typeof(Renderer<>).MakeGenericType(renderInformation.GetType()));

            await renderer.Render(renderInformation, context, theme, renderTo);

            context.ExitContext(settingsContextId);

            foreach (var item in contexts)
                context.ExitContext(item.Key);
        }

        public async Task RenderTemplate(CmsTemplate template, IDictionary<string, object> settings, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            if (!context.CanRender(template, theme))
                return;

            var templateSettings = template.Settings.ToDictionary(x => x.Key, x => x.Value);

            foreach (var item in settings)
                templateSettings[item.Key] = item.Value;

            var contextId = Guid.NewGuid();
            context.EnterContext(contextId, new TemplateSettingsContext(templateSettings));

            await ParseText(template.Body, context, theme, renderTo);

            context.ExitContext(contextId);
        }

        public async Task ParseText(string text, ICmsContext context, ITheme theme, TextWriter renderTo)
        {
            foreach (var textParser in _textParsers)
                text = await textParser.Parse(text, context, theme);

            renderTo.Write(text);
        }

        private interface IRenderer
        {
            Task Render(IRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo);
        }

        private class Renderer<TRenderInformation> : IRenderer where TRenderInformation : IRenderInformation
        {
            private readonly ICmsRenderer<TRenderInformation> _renderer;

            public Renderer(ICmsRenderer<TRenderInformation> renderer)
            {
                _renderer = renderer;
            }

            public Task Render(IRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
            {
                return _renderer.Render((TRenderInformation)information, context, theme, renderTo);
            }
        }
    }
}