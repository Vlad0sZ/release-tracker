namespace Runtime.Interfaces.Logging
{
    /// <summary>
    /// Defines a contract for creating logger instances for specific contexts or categories.
    /// Implementations should provide loggers tailored to different parts of the application (e.g., UI, data, animations).
    /// </summary>
    public interface ILogFactory
    {
        /// <summary>
        /// Creates a logger instance for a specific type, typically used to log messages associated with a class or component.
        /// </summary>
        /// <typeparam name="T">The type associated with the logger (e.g., a MonoBehaviour or service class).</typeparam>
        /// <returns>An ILogger instance for the specified type.</returns>
        ILogger CreateLogger<T>();

        /// <summary>
        /// Creates a logger instance for a specific category, allowing logs to be grouped by custom categories (e.g., "UI", "Data", "Animation").
        /// </summary>
        /// <param name="category">The category name for the logger.</param>
        /// <returns>An ILogger instance for the specified category.</returns>
        ILogger CreateLogger(string category);
    }
}