namespace Runtime.Interfaces.Navigations
{
    public interface INavigationScreenPayload<in TPayload> : INavigationScreen
    {
        void BindPayload(TPayload payload);
    }
}