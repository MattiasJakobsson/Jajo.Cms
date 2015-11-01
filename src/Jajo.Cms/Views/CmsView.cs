using System;
using System.IO;
using System.Threading.Tasks;

namespace Jajo.Cms.Views
{
    public class CmsView
    {
        private readonly Func<TextWriter, Task> _render;

        public CmsView(Func<TextWriter, Task> render)
        {
            _render = render;
        }

        public Task Render(TextWriter renderTo)
        {
            return _render(renderTo);
        }
    }
}