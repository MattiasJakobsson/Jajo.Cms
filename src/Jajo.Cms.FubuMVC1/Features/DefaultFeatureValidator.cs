using System.IO;
using FubuMVC.Core.Runtime.Files;
using Jajo.Cms.Features;
using Newtonsoft.Json;

namespace Jajo.Cms.FubuMVC1.Features
{
    public class DefaultFeatureValidator : IFeatureValidator
    {
        private static FeatureSettings features;

        private readonly IFubuApplicationFiles _fubuApplicationFiles;

        public DefaultFeatureValidator(IFubuApplicationFiles fubuApplicationFiles)
        {
            _fubuApplicationFiles = fubuApplicationFiles;
        }

        public virtual bool IsActive(string feature)
        {
            return features != null && features.IsActive(feature);
        }

        public virtual void Load()
        {
            var filePath = $"{_fubuApplicationFiles.GetApplicationPath()}\\global.features";

            features = GetFromFile(filePath);
        }

        protected virtual FeatureSettings GetFromFile(string path)
        {
            return !File.Exists(path) ? new FeatureSettings() : JsonConvert.DeserializeObject<FeatureSettings>(File.ReadAllText(path));
        }
    }
}