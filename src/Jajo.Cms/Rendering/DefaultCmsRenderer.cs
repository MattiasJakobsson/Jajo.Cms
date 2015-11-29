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

        public async Task<IRenderResult> RenderEndpoint<TInput>(TInput input, ICmsContext context, ITheme theme) where TInput : ICmsEndpointInput
        {
            var themeEndpoint = context
                .Filter(_themeEndpoints.OfType<ICmsEndpoint<TInput>>(), theme)
                .FirstOrDefault();

            if (themeEndpoint == null)
                return new RenderResult("text/plain", x => Task.Run(() => { }), new Dictionary<Guid, IRequestContext>(), context);

            var settings = themeEndpoint.GetDefaultSettings();
            var endpointConfiguration = await _endpointConfigurationStorage.Load(theme.GetName(), theme);

            if (endpointConfiguration != null)
            {
                foreach (var item in endpointConfiguration.Settings)
                    settings[item.Key] = item.Value;
            }

            var renderInformation = await themeEndpoint.Render(input, context, settings);

            if (renderInformation == null)
                return new RenderResult("text/plain", x => Task.Run(() => { }), new Dictionary<Guid, IRequestContext>(), context);

            var contexts = renderInformation.Contexts.ToDictionary(x => Guid.NewGuid(), x => x);
            contexts[Guid.NewGuid()] = new EndpointSettingsContext(settings);

            return new RenderResult(renderInformation.ContentType, x =>
            {
                var renderer = (IRenderer)context.Resolve(typeof(Renderer<>).MakeGenericType(renderInformation.GetType()));

                return renderer.Render(renderInformation, context, theme, x);
            }, contexts, context);
        }

        public async Task<IRenderResult> RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context, ITheme theme)
        {
            if (!context.CanRender(component, theme))
                return new RenderResult("text/plain", x => Task.Run(() => { }), new Dictionary<Guid, IRequestContext>(), context);

            var renderInformation = await component.Render(context, settings);

            if (renderInformation == null)
                return new RenderResult("text/plain", x => Task.Run(() => { }), new Dictionary<Guid, IRequestContext>(), context);

            var contexts = renderInformation.Contexts.ToDictionary(x => Guid.NewGuid(), x => x);
            contexts[Guid.NewGuid()] = new ComponentSettingsContext(settings);

            return new RenderResult(renderInformation.ContentType, x =>
            {
                var renderer = (IRenderer)context.Resolve(typeof(Renderer<>).MakeGenericType(renderInformation.GetType()));

                return renderer.Render(renderInformation, context, theme, x);
            }, contexts, context);
        }

        public async Task<IRenderResult> RenderTemplate(CmsTemplate template, IDictionary<string, object> settings, ICmsContext context, ITheme theme)
        {
            if (!context.CanRender(template, theme))
                return new RenderResult("text/plain", x => Task.Run(() => { }), new Dictionary<Guid, IRequestContext>(), context);

            var templateSettings = template.Settings.ToDictionary(x => x.Key, x => x.Value);

            foreach (var item in settings)
                templateSettings[item.Key] = item.Value;

            var contexts = new Dictionary<Guid, IRequestContext>
            {
                {Guid.NewGuid(), new TemplateSettingsContext(templateSettings)}
            };

            var result = await ParseText(template.Body, context, theme);

            return new RenderResult(string.IsNullOrEmpty(template.ContentType) ? result.ContentType : template.ContentType, x => result.RenderTo(x), contexts, context);
        }

        public async Task<IRenderResult> ParseText(string text, ICmsContext context, ITheme theme)
        {
            foreach (var textParser in _textParsers)
                text = await textParser.Parse(text, this, context, theme);

            return new RenderResult("text/html", x => x.WriteAsync(text), new Dictionary<Guid, IRequestContext>(), context);
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

        private class RenderResult : IRenderResult
        {
            private readonly Func<TextWriter, Task> _render;
            private readonly IDictionary<Guid, IRequestContext> _requestContexts;
            private readonly ICmsContext _cmsContext;

            public RenderResult(string contentType, Func<TextWriter, Task> render, IDictionary<Guid, IRequestContext> requestContexts, ICmsContext cmsContext)
            {
                ContentType = contentType;
                _render = render;
                _requestContexts = requestContexts;
                _cmsContext = cmsContext;
            }

            public string ContentType { get; private set; }

            public async Task RenderTo(TextWriter writer)
            {
                foreach (var context in _requestContexts)
                    _cmsContext.EnterContext(context.Key, context.Value);

                await _render(writer);

                foreach (var context in _requestContexts)
                    _cmsContext.ExitContext(context.Key);
            }
        }
    }
}