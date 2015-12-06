using System.Collections.Generic;
using System.IO;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Rendering;

namespace Jajo.Cms.FubuMVC1.Endpoints
{
    public class CmsEndpointWriter<T> : IMediaWriter<T> where T : ICmsEndpointInput
    {
        private readonly IOutputWriter _writer;
        private readonly ICmsRenderer _cmsRenderer;
        private readonly ICmsContext _cmsContext;

        public CmsEndpointWriter(IOutputWriter writer, ICmsRenderer cmsRenderer, ICmsContext cmsContext)
        {
            _writer = writer;
            _cmsRenderer = cmsRenderer;
            _cmsContext = cmsContext;
        }

        public IEnumerable<string> Mimetypes
        {
            get
            {
                yield return MimeType.Html.Value;
            }
        }

        public void Write(string mimeType, T resource)
        {
            var renderResult = _cmsRenderer.RenderEndpoint(resource, _cmsContext, _cmsContext.GetCurrentTheme());

            _writer.Write(renderResult.ContentType, x =>
            {
                var writer = new StreamWriter(x);

                renderResult.RenderTo(writer);

                writer.Flush();
            });
        }
    }
}