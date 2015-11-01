using System;
using System.Collections.Generic;
using Jajo.Cms.Theme;

namespace Jajo.Cms
{
    public interface ICmsContext
    {
        object Resolve(Type serviceType);
        TService Resolve<TService>();

        IEnumerable<IRequestContext> FindCurrentContexts();
        IRequestContext FindContext(string type);
        TContext FirstContextOf<TContext>(Func<TContext, bool> filter = null) where TContext : IRequestContext;
        TContext LastContextOf<TContext>(Func<TContext, bool> filter = null) where TContext : IRequestContext;
        IEnumerable<TContext> FindContexts<TContext>(Func<TContext, bool> filter = null) where TContext : IRequestContext;
        void EnterContext(Guid id, IRequestContext context);
        void ExitContext(Guid id);
        bool HasContext<TContext>() where TContext : IRequestContext;
        bool HasContext(Type contextType);

        IEnumerable<TInput> Filter<TInput>(IEnumerable<TInput> input, ITheme theme);
        bool CanRender(object input, ITheme theme);

        ITheme GetCurrentTheme();
    }
}