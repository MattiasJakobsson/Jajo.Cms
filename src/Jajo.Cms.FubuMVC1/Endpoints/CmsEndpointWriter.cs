using System.Collections.Generic;
using System.IO;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using Jajo.Cms.Endpoints;
using Jajo.Cms.Rendering;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1.Endpoints
{
    public class CmsEndpointWriter : IMediaWriter<ICmsEndpointInput>
    {
        private readonly IOutputWriter _writer;
        private readonly ICmsRenderer _cmsRenderer;
        private readonly ICmsContext _cmsContext;
        private readonly ITheme _theme;

        public CmsEndpointWriter(IOutputWriter writer, ICmsRenderer cmsRenderer, ICmsContext cmsContext, ITheme theme)
        {
            _writer = writer;
            _cmsRenderer = cmsRenderer;
            _cmsContext = cmsContext;
            _theme = theme;
        }

        public IEnumerable<string> Mimetypes
        {
            get
            {
                yield return MimeType.Any.Value;
            }
        }

        public void Write(string mimeType, ICmsEndpointInput resource)
        {
            var renderResult = _cmsRenderer.RenderEndpoint(resource, _cmsContext, _theme).Result;

            _writer.Write(renderResult.ContentType, x => renderResult.RenderTo(new StreamWriter(x)).Wait());
        }
    }
}