using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Runtime.Interfaces.IO;
using Runtime.Interfaces.Logging;

namespace Runtime.IO
{
    /// <summary>
    /// File helper for read/write as JSON-format.
    /// </summary>
    [UsedImplicitly]
    internal sealed class FileHelperJson : IFileHelper
    {
        private readonly ILogger<IFileHelper> _logger;

        /// <summary>
        /// Constructor with logger;
        /// </summary>
        /// <param name="logger">logger.</param>
        public FileHelperJson(ILogger<IFileHelper> logger) =>
            _logger = logger;

        /// <summary>
        /// Load data-class from file as JSON.
        /// </summary>
        /// <param name="path">Full-path to file with JSON inside.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="T">Type of data-class.</typeparam>
        /// <returns>Data-class instance, when JSON is valid.</returns>
        /// <exception cref="FileNotFoundException">When file does not exists at path.</exception>
        public async UniTask<T> LoadFromFileAsync<T>(string path, CancellationToken cancellationToken)
        {
            if (File.Exists(path) == false)
                throw new FileNotFoundException($"File at path {path} does not exists");

            try
            {
                string data = await File.ReadAllTextAsync(path, cancellationToken);
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return default;
            }
        }

        /// <summary>
        /// Save data-class to file as JSON.
        /// File will be overwrite.
        /// </summary>
        /// <param name="path">Full-path to file.</param>
        /// <param name="context">Data-class with data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="T">Type of data-class.</typeparam>
        public async UniTask SaveToFileAsync<T>(string path, T context, CancellationToken cancellationToken)
        {
            try
            {
                var data = JsonConvert.SerializeObject(context);
                await File.WriteAllTextAsync(path, data, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        public UniTask RemoveFile(string path, CancellationToken cancellationToken)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }

            return UniTask.CompletedTask;
        }
    }
}