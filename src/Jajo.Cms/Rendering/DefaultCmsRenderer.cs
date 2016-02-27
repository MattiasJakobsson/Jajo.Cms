using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public virtual IRenderResult RenderEndpoint<TInput>(TInput input, ICmsContext context, ITheme theme) where TInput : ICmsEndpointInput
        {
            var themeEndpoint = context
                .Filter(_themeEndpoints.OfType<ICmsEndpoint<TInput>>(), theme)
                .FirstOrDefault();

            if (themeEndpoint == null)
                return new RenderResult("text/plain", x => { }, new Dictionary<Guid, RequestContext>(), context);

            var settings = themeEndpoint.GetDefaultSettings();
            var endpointConfiguration = _endpointConfigurationStorage.Load(theme.GetName(), theme);

            if (endpointConfiguration != null)
            {
                foreach (var item in endpointConfiguration.Settings)
                    settings[item.Key] = item.Value;
            }

            var renderInformation = themeEndpoint.Render(input, context, settings);

            if (renderInformation == null)
                return new RenderResult("text/plain", x => { }, new Dictionary<Guid, RequestContext>(), context);

            var contexts = renderInformation.Contexts.ToDictionary(x => Guid.NewGuid(), x => x);
            contexts[Guid.NewGuid()] = EndpointSettingsContext.Build(settings);

            return new RenderResult(renderInformation.ContentType, x =>
            {
                var renderer = (IRenderer)context.Resolve(typeof(Renderer<>).MakeGenericType(renderInformation.GetType()));

                renderer.Render(renderInformation, context, theme, x);
            }, contexts, context);
        }

        public virtual IRenderResult RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context, ITheme theme)
        {
            if (!context.CanRender(component, theme))
                return new RenderResult("text/plain", x => { }, new Dictionary<Guid, RequestContext>(), context);

            var renderInformation = component.Render(context, settings);

            if (renderInformation == null)
                return new RenderResult("text/plain", x => { }, new Dictionary<Guid, RequestContext>(), context);

            var contexts = renderInformation.Contexts.ToDictionary(x => Guid.NewGuid(), x => x);
            contexts[Guid.NewGuid()] = ComponentSettingsContext.Build(settings);

            return new RenderResult(renderInformation.ContentType, x =>
            {
                var renderer = (IRenderer)context.Resolve(typeof(Renderer<>).MakeGenericType(renderInformation.GetType()));

                renderer.Render(renderInformation, context, theme, x);
            }, contexts, context);
        }

        public virtual IRenderResult RenderTemplate(CmsTemplate template, IDictionary<string, object> settings, ICmsContext context, ITheme theme)
        {
            if (!context.CanRender(template, theme))
                return new RenderResult("text/plain", x => { }, new Dictionary<Guid, RequestContext>(), context);

            var contexts = new Dictionary<Guid, RequestContext>();

            var result = ParseText($"<md>{template.Body}</md>", context, theme);

            return new RenderResult(string.IsNullOrEmpty(template.ContentType) ? result.ContentType : template.ContentType, x => result.RenderTo(x), contexts, context);
        }

        public virtual IRenderResult ParseText(string text, ICmsContext context, ITheme theme, ParseTextOptions options = null)
        {
            var parsers = _textParsers.ToList();

            if (options != null && options.UseOnlyParsersTagged.Any())
                parsers = parsers.Where(x => options.UseOnlyParsersTagged.All(y => x.GetTags().Contains(y))).ToList();

            foreach (var textParser in parsers)
            {
                var useOptionsForNextLevel = options != null && !options.FilterOnlyFirstLevel;

                try
                {
                    text = textParser.Parse(text, this, context, theme, x => ParseText(x, context, theme, useOptionsForNextLevel ? options : null).Read());
                }
                catch (Exception)
                {
                    //TODO:Log exception
                }
            }

            return new RenderResult("text/html", x => x.Write(text), new Dictionary<Guid, RequestContext>(), context);
        }

        private interface IRenderer
        {
            void Render(IRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo);
        }

        private class Renderer<TRenderInformation> : IRenderer where TRenderInformation : IRenderInformation
        {
            private readonly ICmsRenderer<TRenderInformation> _renderer;

            public Renderer(ICmsRenderer<TRenderInformation> renderer)
            {
                _renderer = renderer;
            }

            public void Render(IRenderInformation information, ICmsContext context, ITheme theme, TextWriter renderTo)
            {
                _renderer.Render((TRenderInformation)information, context, theme, renderTo);
            }
        }

        private class RenderResult : IRenderResult
        {
            private readonly Action<TextWriter> _render;
            private readonly IDictionary<Guid, RequestContext> _requestContexts;
            private readonly ICmsContext _cmsContext;

            public RenderResult(string contentType, Action<TextWriter> render, IDictionary<Guid, RequestContext> requestContexts, ICmsContext cmsContext)
            {
                ContentType = contentType;
                _render = render;
                _requestContexts = requestContexts;
                _cmsContext = cmsContext;
            }

            public string ContentType { get; }

            public void RenderTo(TextWriter writer)
            {
                foreach (var context in _requestContexts)
                    _cmsContext.EnterContext(context.Key, context.Value);

                _render(writer);

                foreach (var context in _requestContexts)
                    _cmsContext.ExitContext(context.Key);
            }

            public string Read()
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);

                RenderTo(writer);

                writer.Flush();
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}