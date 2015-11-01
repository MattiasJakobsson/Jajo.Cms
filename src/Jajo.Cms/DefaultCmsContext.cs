using System;
using System.Collections.Generic;
using System.Linq;
using Jajo.Cms.Features;
using Jajo.Cms.Theme;

namespace Jajo.Cms
{
    public class DefaultCmsContext : ICmsContext
    {
        private readonly IDictionary<Guid, IRequestContext> _currentContexts = new Dictionary<Guid, IRequestContext>();
        private readonly Func<Type, object> _resolve;
        private readonly IFeatureValidator _featureValidator;
        private readonly ITheme _theme;

        public DefaultCmsContext(Func<Type, object> resolve, IFeatureValidator featureValidator, ITheme theme)
        {
            _resolve = resolve;
            _featureValidator = featureValidator;
            _theme = theme;
        }

        public object Resolve(Type serviceType)
        {
            return _resolve(serviceType);
        }

        public TService Resolve<TService>()
        {
            return (TService)_resolve(typeof(TService));
        }

        public IEnumerable<IRequestContext> FindCurrentContexts()
        {
            return _currentContexts.Select(x => x.Value).ToList();
        }

        public IRequestContext FindContext(string type)
        {
            return _currentContexts.Where(x => IsContextMatch(x.Value.GetType(), type)).Select(x => x.Value).FirstOrDefault();
        }

        public TContext FirstContextOf<TContext>(Func<TContext, bool> filter = null) where TContext : IRequestContext
        {
            return FindContexts(filter).FirstOrDefault();
        }

        public TContext LastContextOf<TContext>(Func<TContext, bool> filter = null) where TContext : IRequestContext
        {
            return FindContexts(filter).LastOrDefault();
        }

        public IEnumerable<TContext> FindContexts<TContext>(Func<TContext, bool> filter = null) where TContext : IRequestContext
        {
            filter = filter ?? (x => true);

            return FindCurrentContexts().OfType<TContext>().Where(filter);
        }

        public void EnterContext(Guid id, IRequestContext context)
        {
            _currentContexts[id] = context;
        }

        public void ExitContext(Guid id)
        {
            if (_currentContexts.ContainsKey(id))
                _currentContexts.Remove(id);
        }

        public bool HasContext<TContext>() where TContext : IRequestContext
        {
            return HasContext(typeof(TContext));
        }

        public bool HasContext(Type contextType)
        {
            return _currentContexts.Any(x => x.Value.GetType() == contextType);
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

        public ITheme GetCurrentTheme()
        {
            return _theme;
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

        private static bool IsContextMatch(Type contextType, string requiredType)
        {
            var currentType = contextType;

            while (currentType != null)
            {
                if (string.Equals(currentType.Name, requiredType, StringComparison.OrdinalIgnoreCase))
                    return true;

                currentType = currentType.BaseType;
            }

            return contextType.GetInterfaces().Any(x => string.Equals(x.Name, requiredType, StringComparison.OrdinalIgnoreCase));
        }
    }
}