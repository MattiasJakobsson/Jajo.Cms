using Jajo.Cms.Templates;

namespace Jajo.Cms.FubuMVC1
{
    public class DefaultTemplateStorage : ITemplateStorage
    {
        private readonly ICmsContext _cmsContext;

        public DefaultTemplateStorage(ICmsContext cmsContext)
        {
            _cmsContext = cmsContext;
        }

        public CmsTemplate Load(string name)
        {
            return new CmsTemplate(name, "", "text/html", _cmsContext.GetCurrentTheme().GetType());
        }
    }
}