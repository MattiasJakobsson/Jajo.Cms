﻿using System;
using System.Collections.Generic;
using System.Web;
using FubuMVC.Core.View;
using Jajo.Cms.Components;
using Jajo.Cms.Rendering;
using Jajo.Cms.Templates;
using Jajo.Cms.Theme;
using NLog;

namespace Jajo.Cms.FubuMVC1
{
    public static class CmsFubuPageExtensions
    {
        public static IHtmlString Template(this IFubuPage page, CmsTemplate template, IDictionary<string, object> settings = null, ITheme theme = null)
        {
            var cmsRenderer = page.ServiceLocator.GetInstance<ICmsRenderer>();
            var cmsContext = page.ServiceLocator.GetInstance<ICmsContext>();

            theme = theme ?? cmsContext.GetCurrentTheme();

            var result = cmsRenderer.RenderTemplate(template, settings, cmsContext, theme);

            return new HtmlString(result.Read());
        }

        public static IHtmlString Component(this IFubuPage page, ICmsComponent component, IDictionary<string, object> settings = null, ITheme theme = null)
        {
            var cmsRenderer = page.ServiceLocator.GetInstance<ICmsRenderer>();
            var cmsContext = page.ServiceLocator.GetInstance<ICmsContext>();

            theme = theme ?? cmsContext.GetCurrentTheme();

            var result = cmsRenderer.RenderComponent(component, settings ?? new Dictionary<string, object>(), cmsContext, theme);

            return new HtmlString(result.Read());
        }

        public static IHtmlString ParseText(this IFubuPage page, string text, ParseTextOptions options = null, ITheme theme = null)
        {
            try
            {
                var cmsRenderer = page.ServiceLocator.GetInstance<ICmsRenderer>();
                var cmsContext = page.ServiceLocator.GetInstance<ICmsContext>();

                theme = theme ?? cmsContext.GetCurrentTheme();

                var result = cmsRenderer.ParseText(text, cmsContext, theme, options);

                return new HtmlString(result.Read());
            }
            catch (Exception ex)
            {
                var logger = page.ServiceLocator.GetInstance<Logger>();

                logger.Error(ex, "Failed parsing text!");

                return new HtmlString(text);
            }
        }
    }
}