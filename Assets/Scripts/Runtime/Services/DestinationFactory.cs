using System;
using Runtime.Interfaces.Services;
using Unity.AppUI.MVVM;
using Unity.AppUI.Navigation;

namespace Runtime.Services
{
    internal sealed class DestinationFactory : IDestinationFactory
    {
        public NavigationScreen CreateDestination(Type type)
        {
            var screen = App.current.services.GetService(type) as NavigationScreen;
            UnityEngine.Debug.Assert(screen != null, $"Can not resolve service  of type {type}");

            return screen;
        }
    }
}