using R3;

namespace Runtime.Interfaces.UI
{
    public interface IClickableBindable<T> : IBindable<T>
    {
        void AddOnClickListener(System.Action<T> onClick);
    }
}