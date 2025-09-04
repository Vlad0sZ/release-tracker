using System.Runtime.CompilerServices;
using UnityEngine;

namespace Runtime.Logging.Unity
{
    /// <summary>
    /// A logger implementation that outputs messages to Unity's Debug console.
    /// Suitable for debugging and logging in the Release Tracker application during development and runtime.
    /// </summary>
    internal sealed class UnityDebugLogger : Runtime.Interfaces.Logging.ILogger
    {
        private readonly string _category;

        /// <summary>
        /// Initializes a new instance of the UnityDebugLogger with a specified category.
        /// </summary>
        /// <param name="category">The category name for the logger (e.g., class name or module).</param>
        public UnityDebugLogger(string category) =>
            _category = category;

        /// <summary>
        /// Logs an informational message to the Unity Debug console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogInfo(string message) =>
            Debug.Log($"[{_category}] INFO: {message}");

        /// <summary>
        /// Logs a warning message to the Unity Debug console.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogWarning(string message) =>
            Debug.LogWarning($"[{_category}] WARNING: {message}");

        /// <summary>
        /// Logs an error message to the Unity Debug console.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(string message) =>
            Debug.LogError($"[{_category}] ERROR: {message}");

        /// <summary>
        /// Logs an exception with an optional custom message to the Unity Debug console.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">An optional message providing context for the exception.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogException(System.Exception exception, string message = null) =>
            Debug.LogException(string.IsNullOrEmpty(message)
                ? exception
                : new System.Exception($"[{_category}] {message}", exception));
    }
}