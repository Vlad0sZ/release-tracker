using System;
using Runtime.Interfaces.Services;
using Unity.AppUI.Navigation;

namespace Runtime.Services
{
    internal sealed class DestinationFactory : IDestinationFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITemplateLoader _templateLoader;

        public DestinationFactory(IServiceProvider serviceProvider, ITemplateLoader templateLoader)
        {
            _serviceProvider = serviceProvider;
            _templateLoader = templateLoader;
        }

        public NavigationScreen CreateDestination(Type type)
        {
            var screen = _serviceProvider.GetService(type) as NavigationScreen;
            UnityEngine.Debug.Assert(screen != null, $"Can not resolve service  of type {type}");

            if (screen is ITemplateScreen templateScreen)
            {
                var template = _templateLoader.GetTemplate(type.Name);
                if (template != null)
                    templateScreen.ApplyTemplate(template);
            } 
                
            
            return screen;
        }
    }
}