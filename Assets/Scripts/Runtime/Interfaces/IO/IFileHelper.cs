using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Interfaces.IO
{
    /// <summary>
    /// Interface for write/read files from filesystem.
    /// </summary>
    public interface IFileHelper
    {
        /// <summary>
        /// Async loading file from path.
        /// </summary>
        /// <param name="path">Full-path to file.</param>
        /// <param name="cancellationToken">Cancellation token for async-operation.</param>
        /// <typeparam name="T">Type of file context.</typeparam>
        /// <returns>File as instance of class.</returns>
        UniTask<T> LoadFromFileAsync<T>(string path, CancellationToken cancellationToken);

        /// <summary>
        /// Async save data context to file.
        /// </summary>
        /// <param name="path">Full-path to file.</param>
        /// <param name="context">Instance of class or data.</param>
        /// <param name="cancellationToken">Cancellation token for async-operation.</param>
        /// <typeparam name="T">Type of class context.</typeparam>
        /// <returns><see cref="UniTask"/> of operation.</returns>
        UniTask SaveToFileAsync<T>(string path, T context, CancellationToken cancellationToken);
    }
}