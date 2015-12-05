using System;
using Jajo.Cms.Localization;
using UStack.Infrastructure.UnitOfWork;

namespace Jajo.Cms.FubuMVC1.Localization
{
    public class LoadLocalizationsOnStartup : IApplicationTask
    {
        private readonly ILocalizeText _localizeText;

        public LoadLocalizationsOnStartup(ILocalizeText localizeText)
        {
            _localizeText = localizeText;
        }

        public void Start()
        {
            _localizeText.Load();
        }

        public void ShutDown()
        {
            
        }

        public void Exception(Exception exception)
        {
            
        }
    }
}