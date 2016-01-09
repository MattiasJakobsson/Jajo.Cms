using System;
using System.Collections.Generic;
using System.Linq;
using Jajo.Cms.Features;
using Jajo.Cms.Theme;

namespace Jajo.Cms
{
    public class DefaultCmsContext : ICmsContext
    {
        private readonly IDictionary<Guid, RequestContext> _currentContexts = new Dictionary<Guid, RequestContext>();
        private readonly Func<Type, object> _resolve;
        private readonly IFeatureValidator _featureValidator;
        private readonly IEnumerable<ITheme> _themes;
        private readonly Func<string> _findCategoryForCurrentContext;

        public DefaultCmsContext(Func<Type, object> resolve, Func<string> findCategoryForCurrentContext, IFeatureValidator featureValidator, IEnumerable<ITheme> themes)
        {
            _resolve = resolve;
            _featureValidator = featureValidator;
            _themes = themes;
            _findCategoryForCurrentContext = findCategoryForCurrentContext;
        }

        public object Resolve(Type serviceType)
        {
            return _resolve(serviceType);
        }

        public TService Resolve<TService>()
        {
            return (TService)_resolve(typeof(TService));
        }

        public IEnumerable<RequestContext> FindCurrentContexts()
        {
            return _currentContexts.Select(x => x.Value).ToList();
        }

        public RequestContext FindContext(string name)
        {
            return _currentContexts.Where(x => x.Value.Name == name).Select(x => x.Value).FirstOrDefault();
        }

        public RequestContext FirstContextOf(string name, Func<RequestContext, bool> filter = null)
        {
            return FindContexts(name, filter).FirstOrDefault();
        }

        public RequestContext LastContextOf(string name, Func<RequestContext, bool> filter = null)
        {
            return FindContexts(name, filter).LastOrDefault();
        }

        public IEnumerable<RequestContext> FindContexts(string name, Func<RequestContext, bool> filter = null)
        {
            filter = filter ?? (x => true);

            return FindCurrentContexts().Where(x => x.Name == name && filter(x));
        }

        public void EnterContext(Guid id, RequestContext context)
        {
            _currentContexts[id] = context;
        }

        public void ExitContext(Guid id)
        {
            if (_currentContexts.ContainsKey(id))
                _currentContexts.Remove(id);
        }

        public bool HasContext(string name)
        {
            return _currentContexts.Any(x => x.Value.Name == name);
        }

        public IEnumerable<TInput> Filter<TInput>(IEnumerable<TInput> input, ITheme theme)
        {
            var result = input.Where(x => HasRequiredContextsFor(x) && IsFeaturesEnabledFor(x)).ToList();

            foreach (var item in GetThemeTypes(theme).SelectMany(type => result.Where(x => DoesItemBelongToTheme(x, type))))
                yield return item;

            foreach (var item in result.Where(x => DoesItemBelongToTheme(x, null)))
                yield return item;
        }

        public bool CanRender(object input, ITheme theme)
        {
            if (!HasRequiredContextsFor(input) || !IsFeaturesEnabledFor(input))
                return false;

            var themeTypes = GetThemeTypes(theme).ToList();
            themeTypes.Add(null);

            return themeTypes.Any(x => DoesItemBelongToTheme(input, x));
        }

        public ITheme GetCurrentTheme(string category = null)
        {
            category = category ?? _findCategoryForCurrentContext();

            return _themes.FirstOrDefault(x => x.GetCategory() == category && x.IsActive());
        }

        private bool IsFeaturesEnabledFor(object input)
        {
            var belongToFeatures = input as IBelongToFeatures;

            return belongToFeatures == null || belongToFeatures.GetFeatures().All(x => _featureValidator.IsActive(x));
        }

        private bool HasRequiredContextsFor(object input)
        {
            var requireContexts = input as IRequireContexts;

            return requireContexts == null || requireContexts.GetRequiredContexts().All(HasContext);
        }

        private static bool DoesItemBelongToTheme(object item, Type themeType)
        {
            var belongsToTheme = item as IBelongToTheme;

            if (belongsToTheme == null && themeType == null)
                return true;

            if (belongsToTheme == null || themeType == null)
                return false;

            return belongsToTheme.GetTheme() == themeType;
        }

        private static IEnumerable<Type> GetThemeTypes(ITheme theme)
        {
            if (theme == null)
                yield break;

            yield return theme.GetType();

            var parentType = theme.GetType().BaseType;

            while (parentType != null)
            {
                if (typeof(ITheme).IsAssignableFrom(parentType))
                    yield return parentType;

                parentType = parentType.BaseType;
            }
        }
    }
}