using System.Collections.Generic;
using Jajo.Cms.Theme;

namespace Jajo.Cms.Views
{
    public interface ICmsViewEngine
    {
        CmsView FindView<TModel>(string viewName, TModel model, ITheme theme, IEnumerable<IRequestContext> contexts) where TModel : class;
    }
}