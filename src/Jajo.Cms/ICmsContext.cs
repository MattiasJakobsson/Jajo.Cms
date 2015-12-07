using System;
using System.Collections.Generic;
using Jajo.Cms.Theme;

namespace Jajo.Cms
{
    public interface ICmsContext
    {
        object Resolve(Type serviceType);
        TService Resolve<TService>();

        IEnumerable<RequestContext> FindCurrentContexts();
        RequestContext FindContext(string name);
        RequestContext FirstContextOf(string name, Func<RequestContext, bool> filter = null);
        RequestContext LastContextOf(string name, Func<RequestContext, bool> filter = null);
        IEnumerable<RequestContext> FindContexts(string name, Func<RequestContext, bool> filter = null);
        void EnterContext(Guid id, RequestContext context);
        void ExitContext(Guid id);
        bool HasContext(string name);

        IEnumerable<TInput> Filter<TInput>(IEnumerable<TInput> input, ITheme theme);
        bool CanRender(object input, ITheme theme);

        ITheme GetCurrentTheme();
    }
}