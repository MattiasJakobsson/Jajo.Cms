using System.Collections.Generic;
using System.Linq;

namespace Jajo.Cms.FubuMVC1.Features
{
    public class FeatureSettings
    {
        public FeatureSettings()
        {
            Features = new List<Feature>();
        }

        public IEnumerable<Feature> Features { get; set; }

        public bool IsActive(string feature)
        {
            var parts = new Queue<string>(feature.Split('/'));
            Feature currentFeature = null;

            while (parts.Any())
            {
                var currentPart = parts.Dequeue();

                currentFeature = currentFeature == null
                    ? Features.FirstOrDefault(x => x.Name == currentPart)
                    : currentFeature.Children.FirstOrDefault(x => x.Name == currentPart);

                if (currentFeature == null || !currentFeature.Active)
                    return false;
            }

            return true;
        }

        public class Feature
        {
            public Feature()
            {
                Children = new List<Feature>();
            }

            public string Name { get; set; }
            public IEnumerable<Feature> Children { get; set; }
            public bool Active { get; set; }
        }
    }
}