using System.Collections.Generic;

namespace Jajo.Cms.Rendering
{
    public class TextRenderInformation : IRenderInformation
    {
        public TextRenderInformation(IEnumerable<IRequestContext> contexts, string text)
        {
            Contexts = contexts;
            Text = text;
        }

        public IEnumerable<IRequestContext> Contexts { get; private set; }
        public string Text { get; private set; }
    }
}