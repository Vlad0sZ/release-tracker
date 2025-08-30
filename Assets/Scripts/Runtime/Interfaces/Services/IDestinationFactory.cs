using System;
using Unity.AppUI.Navigation;

namespace Runtime.Interfaces.Services
{
    public interface IDestinationFactory
    {
        NavigationScreen CreateDestination(Type type);
    }
}