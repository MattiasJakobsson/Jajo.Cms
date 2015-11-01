using Jajo.Cms.Features;
using StructureMap.Configuration.DSL;

namespace Jajo.Cms.FubuMVC1
{
    public class CmsContextRegistry : Registry
    {
        public CmsContextRegistry()
        {
            For<ICmsContext>().Use(x => new DefaultCmsContext(x.GetInstance, x.GetInstance<IFeatureValidator>()));
        } 
    }
}