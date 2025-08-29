using Runtime.Interfaces.IO;
using Runtime.Interfaces.Logging;
using Runtime.IO;
using Runtime.Logging;
using Runtime.Logging.Unity;
using VContainer;
using VContainer.Unity;

namespace Runtime.Scopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterIO(builder);

            RegisterLogging(builder);
        }

        private static void RegisterIO(IContainerBuilder container)
        {
            container.Register<IFileHelper, FileHelperJson>(Lifetime.Singleton);
            container.Register<IPathHelper, PathHelper>(Lifetime.Singleton);
            container.Register<IDirectoryHelper, DirectoryHelper>(Lifetime.Singleton);
        }

        private static void RegisterLogging(IContainerBuilder container)
        {
            container.Register<ILogFactory, UnityLogFactory>(Lifetime.Singleton);

            container.Register(typeof(ILogger<>),
                typeof(Logger<>), Lifetime.Scoped);
        }
    }
}