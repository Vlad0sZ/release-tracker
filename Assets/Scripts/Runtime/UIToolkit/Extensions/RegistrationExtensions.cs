using Runtime.Interfaces.IO;
using Runtime.Interfaces.Logging;
using Runtime.IO;
using Runtime.Logging;
using Runtime.Logging.Unity;
using Unity.AppUI.MVVM;

namespace Runtime.UIToolkit.Extensions
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddIOServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFileHelper, FileHelperJson>();
            serviceCollection.AddSingleton<IPathHelper, PathHelper>();
            serviceCollection.AddSingleton<IDirectoryHelper, DirectoryHelper>();

            return serviceCollection;
        }

        public static IServiceCollection AddLogging(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILogFactory, UnityLogFactory>();
            serviceCollection.AddScoped(typeof(ILogger<>), typeof(Logger<>));

            return serviceCollection;
        }
    }
}