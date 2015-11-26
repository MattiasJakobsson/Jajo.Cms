using System;
using System.IO;
using System.Threading.Tasks;

namespace Jajo.Cms.Views
{
    public class CmsView
    {
        private readonly Func<TextWriter, string, Task> _render;

        public CmsView(Func<TextWriter, string, Task> render)
        {
            _render = render;
        }

        public Task Render(TextWriter renderTo, string contentType)
        {
            return _render(renderTo, contentType);
        }
    }
}