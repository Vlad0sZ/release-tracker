namespace Runtime.Interfaces.Navigations
{
    public interface INavigationScreen : INavigationDestination
    {
        void Show();

        void Hide();
    }
}