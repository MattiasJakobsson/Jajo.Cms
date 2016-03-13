using System;
using Jajo.Cms.Features;
using UStack.Infrastructure.UnitOfWork;

namespace Jajo.Cms.FubuMVC1.Features
{
    public class LoadFeaturesOnStartup : IApplicationTask
    {
        private readonly IFeatureValidator _featureValidator;

        public LoadFeaturesOnStartup(IFeatureValidator featureValidator)
        {
            _featureValidator = featureValidator;
        }

        public void Start()
        {
            _featureValidator.Load();
        }

        public void ShutDown()
        {

        }

        public void Exception(Exception exception)
        {

        }
    }
}