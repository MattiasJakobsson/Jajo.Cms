using Jajo.Cms.Templates;

namespace Jajo.Cms.FubuMVC1
{
    public class DefaultTemplateStorage : ITemplateStorage
    {
        public CmsTemplate Load(string name)
        {
            return new CmsTemplate();
        }
    }
}