﻿using System.Collections.Generic;

namespace Jajo.Cms.Rendering
{
    public class TextRenderInformation : IRenderInformation
    {
        public TextRenderInformation(IEnumerable<RequestContext> contexts, string text, string contentType)
        {
            Contexts = contexts;
            Text = text;
            ContentType = contentType;
        }

        public string ContentType { get; private set; }
        public IEnumerable<RequestContext> Contexts { get; private set; }
        public string Text { get; private set; }
    }
}