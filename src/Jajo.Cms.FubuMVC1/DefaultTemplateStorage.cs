using Jajo.Cms.Templates;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1
{
    public class DefaultTemplateStorage : ITemplateStorage
    {
        public CmsTemplate Load(string name, ITheme theme)
        {
            return new CmsTemplate(name, "", "text/html", theme.GetType());
        }
    }
}