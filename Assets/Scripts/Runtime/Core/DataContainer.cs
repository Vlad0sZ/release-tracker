using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.IO;
using Runtime.Interfaces.Loaders;
using Runtime.Interfaces.Logging;

namespace Runtime.Core
{
    /// <summary>
    /// Container for ReleaseData.
    /// <remarks>
    ///  implement <see cref="IInitializeLoader"/> when initialized it loads all files into memory.
    /// </remarks>
    /// </summary>
    [UsedImplicitly]
    public sealed class DataContainer : IDataContainer, IInitializeLoader
    {
        /// <summary>
        /// Number of Parallel operation of loading.
        /// </summary>
        private const int ParallelTasks = 5;

        private readonly IPathHelper _pathHelper;
        private readonly IDirectoryHelper _directoryHelper;
        private readonly IFileHelper _fileHelper;
        private readonly ILogger<DataContainer> _logger;

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(ParallelTasks);
        private readonly List<DataClass> _dataClassList = new List<DataClass>(128);

        public IList<DataClass> Data => _dataClassList;

        public DataContainer(IPathHelper pathHelper, IDirectoryHelper directoryHelper, IFileHelper fileHelper,
            ILogger<DataContainer> logger)
        {
            _pathHelper = pathHelper;
            _fileHelper = fileHelper;
            _logger = logger;
            _directoryHelper = directoryHelper;
        }

        public async UniTask Initialize(CancellationToken cancellationToken) =>
            await InitializeParallel(cancellationToken);

        private async UniTask InitializeParallel(CancellationToken cancellationToken)
        {
            var dataRootPath = _pathHelper.GetDataPath();
            var files = _directoryHelper.GetAllFiles(dataRootPath, "*.json")
                .OrderByDescending(f => f.LastWriteTime)
                .Select(f => f.FullName)
                .ToList();
            
            _logger.LogInfo($"find {files.Count} files to load.");

            var sw = Stopwatch.StartNew();

            var tasks = files.Select(x => LoadFromFile(x, cancellationToken));
            var results = await UniTask.WhenAll(tasks);
            _dataClassList.Clear();
            _dataClassList.AddRange(results.Where(x => x != null));

            await UniTask.Yield();
            sw.Stop();
            _logger.LogInfo($"All loaded in {sw.ElapsedMilliseconds} ms.");
        }


        private async UniTask<DataClass> LoadFromFile(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                await _semaphoreSlim.WaitAsync(cancellationToken);
                var data = await _fileHelper.LoadFromFileAsync<DataClass>(filePath, cancellationToken);
                return data ?? throw new NullReferenceException("Data was null when parsing from json");
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, $"Error when load {filePath}");
                return null;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }

    public class DataClass
    {
        public int SomeId { get; set; }
    }
}