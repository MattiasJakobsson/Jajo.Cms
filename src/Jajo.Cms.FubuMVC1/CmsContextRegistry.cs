using System.Linq;
using FubuMVC.Core.Http;
using Jajo.Cms.Features;
using Jajo.Cms.Theme;
using StructureMap.Configuration.DSL;

namespace Jajo.Cms.FubuMVC1
{
    public class CmsContextRegistry : Registry
    {
        public CmsContextRegistry()
        {
            For<ICmsContext>().Use(x => new DefaultCmsContext(x.GetInstance, () =>
            {
                var currentChain = x.TryGetInstance<ICurrentChain>();

                if (currentChain == null)
                    return null;

                var attribute = currentChain
                    .Current
                    .FirstCall()
                    .HandlerType
                    .Assembly
                    .GetCustomAttributes(typeof (ThemeCategoryAttribute), true)
                    .OfType<ThemeCategoryAttribute>()
                    .FirstOrDefault();

                return attribute != null ? attribute.Category : null;
            }, x.GetInstance<IFeatureValidator>(), x.GetAllInstances<ITheme>()));
        } 
    }
}