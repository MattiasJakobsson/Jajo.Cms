using System.Collections.Generic;

namespace Jajo.Cms.Files
{
    public interface ITransformFiles
    {
        string Name { get; }
        IEnumerable<ITransformationSetting> GetTransformationSettings();
    }
}