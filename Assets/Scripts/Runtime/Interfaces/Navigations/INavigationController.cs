namespace Runtime.Interfaces.Navigations
{
    public interface INavigationController
    {
        INavigationDestination NavigateTo<T>();
        INavigationDestination NavigateTo<T, TPayload>(TPayload payload);
    }
}