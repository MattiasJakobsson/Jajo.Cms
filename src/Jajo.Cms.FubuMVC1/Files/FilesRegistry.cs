using FubuMVC.Core;
using Jajo.Cms.Files;

namespace Jajo.Cms.FubuMVC1.Files
{
    public class FilesRegistry : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services(x => x.SetServiceIfNone<IUploadFiles, DefaultFileUploader>());
        }
    }
}