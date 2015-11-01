namespace Jajo.Cms.Menu
{
    public interface IAmBreadCrumb
    {
        BreadCrumb Build(IMenuContext menuContext);
        object FindParent();
    }
}