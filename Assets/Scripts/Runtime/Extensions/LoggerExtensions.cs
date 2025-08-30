using Runtime.Interfaces.Logging;
using Runtime.Logging;

namespace Runtime.Extensions
{
    public static class LoggerExtensions
    {
        public static ILogger<T> CreateLoggerOf<T>(this ILogFactory logFactory) =>
            new Logger<T>(logFactory);
    }
}