using Runtime.Interfaces.IO;
using Runtime.IO;
using VContainer;
using VContainer.Unity;

namespace Trash
{
    public class Scope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<IFileHelper, FileHelperJson>(Lifetime.Singleton);
        }
    }
}