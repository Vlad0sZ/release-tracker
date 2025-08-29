using System;

namespace Runtime.Interfaces.UI
{
    public interface IBindable<in T>
    {
        void Bind(T data);
    }
}