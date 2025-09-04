using System;
using System.Runtime.CompilerServices;
using Runtime.Interfaces.Logging;

namespace Runtime.Logging
{
    /// <summary>
    /// A generic logger implementation that delegates logging operations to an inner ILogger instance.
    /// Provides type-specific logging for the Release Tracker application, using the type name as the logging category.
    /// </summary>
    /// <typeparam name="T">The type associated with the logger, used to define the logging category.</typeparam>
    public class Logger<T> : ILogger<T>
    {
        private readonly ILogger _innerLogger;

        /// <summary>
        /// Initializes a new instance of the Logger class using a provided log factory.
        /// </summary>
        /// <param name="logFactory">The factory used to create the inner ILogger instance.</param>
        public Logger(ILogFactory logFactory) =>
            _innerLogger = logFactory.CreateLogger<T>();

        /// <summary>
        /// Logs an informational message to the underlying logger.
        /// </summary>
        /// <param name="message">The message to log.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogInfo(string message) =>
            _innerLogger.LogInfo(message);

        /// <summary>
        /// Logs a warning message to the underlying logger.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogWarning(string message) =>
            _innerLogger.LogWarning(message);

        /// <summary>
        /// Logs an error message to the underlying logger.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(string message) =>
            _innerLogger.LogError(message);

        /// <summary>
        /// Logs an exception with an optional custom message to the underlying logger.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">An optional message providing context for the exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogException(Exception exception, string message = null) =>
            _innerLogger.LogException(exception, message);
    }
}