using Jajo.Cms.Menu;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public class MenuEndpoint
    {
        private readonly IBuildMenu _buildMenu;

        public MenuEndpoint(IBuildMenu buildMenu)
        {
            _buildMenu = buildMenu;
        }

        public MenuEndpointQueryResult MenuQuery(MenuEndpointQueryInput input)
        {
            return new MenuEndpointQueryResult(_buildMenu.Build(input.MenuName, input.CurrentInput));
        }
    }

    public class MenuEndpointQueryInput
    {
        public string MenuName { get; set; }
        public object CurrentInput { get; set; }
    }

    public class MenuEndpointQueryResult
    {
        public MenuEndpointQueryResult(Cms.Menu.Menu menu)
        {
            Menu = menu;
        }

        public Cms.Menu.Menu Menu { get; private set; }
    }
}