using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Jajo.Cms.Theme;
using StructureMap;

namespace Jajo.Cms.FubuMVC1
{
    public class ThemeRouteConstraint : IRouteConstraint
    {
        private readonly Type _requiredActiveTheme;
        private readonly IContainer _container;

        public ThemeRouteConstraint(Type requiredActiveTheme, IContainer container)
        {
            _requiredActiveTheme = requiredActiveTheme;
            _container = container;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var themes = _container.GetAllInstances<ITheme>();

            return themes.Any(x => x.GetType() == _requiredActiveTheme && x.IsActive());
        }
    }
}