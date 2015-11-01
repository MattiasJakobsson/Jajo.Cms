using System.Web;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;

namespace Jajo.Cms.FubuMVC1.Menu
{
    public static class MenuExtensions
    {
        public static IHtmlString Menu(this IFubuPage page, string menu)
        {
            var inputType = page.Get<ICurrentChain>().Current.InputType();
            var input = inputType != null ? page.Get<IFubuRequest>().Get(inputType) : null;

            return page.Partial(new MenuEndpointQueryInput
            {
                MenuName = menu,
                CurrentInput = input
            });
        }
    }
}