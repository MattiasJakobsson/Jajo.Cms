namespace Jajo.Cms.Features
{
    public interface IFeatureValidator
    {
        bool IsActive(string feature);
        void Load();
    }
}