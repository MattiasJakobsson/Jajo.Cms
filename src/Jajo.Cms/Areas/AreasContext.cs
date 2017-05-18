using System.Collections.Generic;

namespace Jajo.Cms.Areas
{
    public class AreasContext
    {
        public static string Name = typeof(AreasContext).Name;

        public static RequestContext Build(IReadOnlyDictionary<string, string> areas)
        {
            return new RequestContext(Name, new Dictionary<string, object>
            {
                {"Areas", areas}
            });
        }

        public static string GetAreaFrom(RequestContext context, string area)
        {
            if (context?.Data == null || !context.Data.ContainsKey("Areas"))
                return "";

            var areas = context.Data["Areas"] as IReadOnlyDictionary<string, string>;

            if (areas == null || !areas.ContainsKey(area))
                return "";

            return areas[area];
        }
    }
}