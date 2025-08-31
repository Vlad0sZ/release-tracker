using Microsoft.Extensions.DependencyInjection;

namespace Unity.AppUI.Extended.DependencyInjection
{
    public interface IAppConfiguration
    {
        IServiceCollection Services { get; }
    }
}