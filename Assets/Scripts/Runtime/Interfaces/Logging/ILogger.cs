namespace Runtime.Interfaces.Logging
{
    /// <summary>
    /// Defines a contract for logging messages and errors in the application.
    /// Implementations should handle logging to various outputs (e.g., console, file, Unity debug log).
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        void LogError(string message);

        /// <summary>
        /// Logs an exception with a custom message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">An optional message providing context for the exception.</param>
        void LogException(System.Exception exception, string message = null);
    }
}