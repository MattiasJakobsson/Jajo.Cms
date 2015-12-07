using Jajo.Cms.Theme;

namespace Jajo.Cms.Templates
{
    public interface ITemplateStorage
    {
        CmsTemplate Load(string name, ITheme theme);
    }
}