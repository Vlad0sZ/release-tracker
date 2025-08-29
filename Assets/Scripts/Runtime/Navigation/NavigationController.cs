using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Interfaces.Logging;
using Runtime.Interfaces.Navigations;

namespace Runtime.Navigation
{
    public class NavigationController : INavigationController
    {
        private readonly ILogger<INavigationController> _logger;
        private readonly Dictionary<Type, INavigationScreen> _screenMap;

        private INavigationScreen _currentScreen = null;

        public NavigationController(IEnumerable<INavigationScreen> screens, ILogger<INavigationController> logger)
        {
            _screenMap = screens.ToDictionary(x => x.GetType());
            _logger = logger;
        }

        public INavigationDestination NavigateTo<T>()
        {
            if (_screenMap.TryGetValue(typeof(T), out var screen))
                return ChangeScreen(screen);

            _logger.LogError($"No screen registered for {typeof(T).Name}");
            throw new NullReferenceException($"No one screen for type {typeof(T).Name}");
        }

        public INavigationDestination NavigateTo<T, TPayload>(TPayload payload)
        {
            if (_screenMap.TryGetValue(typeof(T), out var screen))
            {
                if (screen is INavigationScreenPayload<TPayload> payloadScreen)
                    payloadScreen.BindPayload(payload);
                else
                    _logger.LogWarning($"type of {typeof(T)} does not implement payload type  {typeof(TPayload)}");

                return ChangeScreen(screen);
            }

            _logger.LogError($"No screen registered for {typeof(T).Name}");
            throw new NullReferenceException($"No one screen for type {typeof(T).Name}");
        }


        private INavigationDestination ChangeScreen(INavigationScreen screen)
        {
            _currentScreen?.Hide();
            _currentScreen = screen;
            _currentScreen.Show();

            return _currentScreen;
        }
    }
}