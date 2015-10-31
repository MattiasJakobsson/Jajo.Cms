namespace Jajo.Cms.Parsing
{
    public interface IFindParameterValueFromModel
    {
        object Find(string parameter, object model);
    }
}