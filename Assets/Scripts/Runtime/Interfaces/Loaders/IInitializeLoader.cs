using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Interfaces.Loaders
{
    /// <summary>
    /// Interface for initializing something.
    /// </summary>
    public interface IInitializeLoader
    {
        /// <summary>
        /// Initialize some data.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for operation.</param>
        /// <returns><see cref="UniTask"/></returns>
        UniTask Initialize(CancellationToken cancellationToken);
    }
}