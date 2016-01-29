using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Jajo.Cms.Theme;

namespace Jajo.Cms.FubuMVC1
{
    public class ThemeRouteConstraint : IRouteConstraint
    {
        private readonly Type _requiredActiveTheme;
        private readonly Func<IEnumerable<ITheme>> _getThemes;

        public ThemeRouteConstraint(Type requiredActiveTheme, Func<IEnumerable<ITheme>> getThemes)
        {
            _requiredActiveTheme = requiredActiveTheme;
            _getThemes = getThemes;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var themes = _getThemes();

            return themes.Any(x => x.GetType() == _requiredActiveTheme && x.IsActive());
        }
    }
}