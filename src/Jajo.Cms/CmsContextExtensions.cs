using System;

namespace Jajo.Cms
{
    public static class CmsContextExtensions
    {
        public static TResult GetDataFromContext<TContext, TResult>(this ICmsContext cmsContext, Func<TContext, TResult> getResult) where TContext : class, IRequestContext
        {
            var context = cmsContext.LastContextOf<TContext>();

            return context == null ? default(TResult) : getResult(context);
        }
    }
}