using FubuMVC.Core.Http;
using Jajo.Cms.Localization;

namespace Jajo.Cms.FubuMVC1.Localization
{
    public class DefaultLocalizationNamespaceFinder : IFindCurrentLocalizationNamespace
    {
        private readonly ICurrentChain _currentChain;

        public DefaultLocalizationNamespaceFinder(ICurrentChain currentChain)
        {
            _currentChain = currentChain;
        }

        public string Find()
        {
            var actionCall = _currentChain.Current.FirstCall();

            if (actionCall == null)
                return "";

            var type = actionCall.HandlerType;

            var typeNamespace = type.Namespace;

            if (string.IsNullOrEmpty(typeNamespace))
                return type.Name;

            typeNamespace = typeNamespace.Replace(".", ":");

            return string.Format("{0}:{1}", typeNamespace, type.Name);
        }
    }
}