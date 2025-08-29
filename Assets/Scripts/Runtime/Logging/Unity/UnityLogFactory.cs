using Runtime.Interfaces.Logging;

namespace Runtime.Logging.Unity
{
    /// <summary>
    /// A factory implementation for creating UnityDebugLogger instances.
    /// Provides loggers for specific types or categories in the Release Tracker application.
    /// </summary>
    internal sealed class UnityLogFactory : ILogFactory
    {
        /// <summary>
        /// Creates a logger instance for a specific type, using the type's name as the category.
        /// </summary>
        /// <typeparam name="T">The type associated with the logger (e.g., a MonoBehaviour or service class).</typeparam>
        /// <returns>An ILogger instance for the specified type.</returns>
        public ILogger CreateLogger<T>() => new UnityDebugLogger(typeof(T).Name);

        /// <summary>
        /// Creates a logger instance for a specific category.
        /// </summary>
        /// <param name="category">The category name for the logger (e.g., "UI", "Data", "Animation").</param>
        /// <returns>An ILogger instance for the specified category.</returns>
        public ILogger CreateLogger(string category) => new UnityDebugLogger(category);
    }
}