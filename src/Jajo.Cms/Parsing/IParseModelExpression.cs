namespace Jajo.Cms.Parsing
{
    public interface IParseModelExpression
    {
        ModelExpressionParseResult Parse(string expression, object model);
    }
}