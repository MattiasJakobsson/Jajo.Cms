using System.Collections.Generic;
using System.IO;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Runtime.Formatters;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1.Endpoints
{
    public class ConfigureEndpointBehaviorChain : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(x => x.SetServiceIfNone<IEndpointConfigurationStorage, DefaultEndpointConfigurationStorage>());

            registry.Policies.Add(x =>
            {
                x.Where.ResourceTypeImplements<ICmsEndpointInput>();
                x.ModifyBy(y => y.Output.AddFormatter<EndpointFormatter>().MoveToFront());
                //x.ModifyBy(y => y.Output.AddWriter(typeof(CmsEndpointWriter<>).MakeGenericType(y.ResourceType())));
            });
        }
    }

    public class EndpointFormatter : IFormatter
    {
        private readonly IOutputWriter _writer;
        private readonly ICmsRenderer _cmsRenderer;
        private readonly ICmsContext _cmsContext;
        private readonly ITheme _theme;

        public EndpointFormatter(IOutputWriter writer, ICmsRenderer cmsRenderer, ICmsContext cmsContext, ITheme theme)
        {
            _writer = writer;
            _cmsRenderer = cmsRenderer;
            _cmsContext = cmsContext;
            _theme = theme;
        }

        public void Write<T>(T target, string mimeType)
        {
            var resource = target as ICmsEndpointInput;

            if(resource == null)
                return;

            var renderResult = _cmsRenderer.RenderEndpoint(resource, _cmsContext, _theme).Result;

            _writer.Write(renderResult.ContentType, x => renderResult.RenderTo(new StreamWriter(x)).Wait());
        }

        public T Read<T>()
        {
            return default(T);
        }

        public IEnumerable<string> MatchingMimetypes { get { yield return "text/html"; }}
    }
}