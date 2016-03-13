using System.Collections.Generic;
using System.Linq;
using FubuCore;
using Jajo.Cms.Menu;
using StructureMap;
using UStack.Infrastructure.Authentication;
using UStack.Infrastructure.EndpointValidation;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class DefaultMenuBuilder : IBuildMenu
    {
        private readonly IContainer _container;
        private readonly IServiceLocator _serviceLocator;
        private readonly IValidateSecurity _validateSecurity;
        private readonly IEnumerable<IBuildMenuTree> _buildMenuTrees;
        private readonly IFindBreadCrumbsFor _findBreadCrumbsFor;
        private readonly ICmsContext _cmsContext;

        public DefaultMenuBuilder(IContainer container, IServiceLocator serviceLocator, IValidateSecurity validateSecurity, IEnumerable<IBuildMenuTree> buildMenuTrees, IFindBreadCrumbsFor findBreadCrumbsFor, ICmsContext cmsContext)
        {
            _container = container;
            _serviceLocator = serviceLocator;
            _validateSecurity = validateSecurity;
            _buildMenuTrees = buildMenuTrees;
            _findBreadCrumbsFor = findBreadCrumbsFor;
            _cmsContext = cmsContext;
        }

        public Cms.Menu.Menu Build(string name, object currentInput)
        {
            var breadCrumbs = _findBreadCrumbsFor.Get(currentInput).ToList();

            var menuItems = _buildMenuTrees
                .Where(x => x.ForMenu == name && IsAvailableFor(x))
                .OrderBy(x => x.Order)
                .SelectMany(x => x.Build(currentInput))
                .Where(x => IsAvailableFor(x.Input))
                .Select(x => new Cms.Menu.Menu.MenuItem(x.Title, x.IconName, x.Input, GetChildrenFor(x, currentInput, breadCrumbs), x.IsSelected(currentInput, breadCrumbs)))
                .ToList();

            return new Cms.Menu.Menu(name, menuItems);
        }

        protected IEnumerable<Cms.Menu.Menu.MenuItem> GetChildrenFor(MenuItemSettings item, object currentInput, ICollection<BreadCrumb> breadCrumbs)
        {
            return item
                .Children
                .Where(x => IsAvailableFor(x.Input))
                .Select(x => new Cms.Menu.Menu.MenuItem(x.Title, x.IconName, x.Input, GetChildrenFor(x, currentInput, breadCrumbs), x.IsSelected(currentInput, breadCrumbs)))
                .ToList();
        }

        private bool IsAvailableFor(object input)
        {
            if (input == null)
                return true;

            if (!_cmsContext.CanRender(input, _cmsContext.GetCurrentTheme()))
                return false;

            var ensureExists = _container.GetAllInstances(typeof(IEnsureExists<>).MakeGenericType(input.GetType())).OfType<object>().ToList();

            var exists = ensureExists.All(x => (bool)x.GetType().GetMethod("Exists", new[] { input.GetType() }).Invoke(x, new[] { input }));

            if (!exists)
                return false;

            var secureInterfaces = input.GetType()
                        .GetInterfaces()
                        .Where(x => x.GenericTypeArguments.Length == 1 && x.GenericTypeArguments[0].CanBeCastTo<ISubApplication>() && x.CanBeCastTo(typeof(IAmSecuredBy<>).MakeGenericType(x.GenericTypeArguments[0])))
                        .ToList();

            return (from secureInterface in secureInterfaces
                    let authenticationContext = _serviceLocator.GetInstance(typeof(IAuthenticationService<>).MakeGenericType(secureInterface.GenericTypeArguments[0]))
                    let user = (AuthenticationInformation)authenticationContext.GetType().GetMethod("GetUser").Invoke(authenticationContext, new object[0])
                    select _validateSecurity.IsAllowed(user, (SecuredBy)input
                                                                            .GetType()
                                                                            .GetMethod("GetSecuredBy", new[] { secureInterface.GenericTypeArguments[0] })
                                                                            .Invoke(input, new[] { _serviceLocator.GetInstance(secureInterface.GenericTypeArguments[0]) })))
                     .All(x => x);
        }
    }
}