namespace Jajo.Cms.Localization
{
    public interface ILocalizationVisitor
    {
        string AfterLocalized(string key, string value);
    }
}