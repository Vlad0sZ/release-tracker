using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using ObservableCollections;
using R3;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.IO;
using Runtime.Interfaces.Loaders;
using Runtime.Interfaces.Logging;
using Runtime.Models;

namespace Runtime.Core
{
    /// <summary>
    /// Container for ReleaseData.
    /// <remarks>
    ///  implement <see cref="IInitializeLoader"/> when initialized it loads all files into memory.
    /// </remarks>
    /// </summary>
    [UsedImplicitly]
    public sealed class DataContainer : IDataContainer, IInitializeLoader, IDisposable
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
        private readonly CancellationTokenSource _cts = new();

        public IList<ReleaseInfo> Data { get; set; }

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

        public void Update(ReleaseInfo releaseInfo) =>
            _ = SaveItem(releaseInfo);


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


            var list = new ObservableList<ReleaseInfo>(results);
            var token = _cts.Token;

            list.ObserveAdd()
                .TakeUntil(token)
                .Subscribe(d => _ = SaveItem(d.Value));

            list.ObserveRemove()
                .TakeUntil(token)
                .Subscribe(d => _ = DeleteItem(d.Value));

            Data = list;

            await UniTask.Yield();
            sw.Stop();
            _logger.LogInfo($"All loaded in {sw.ElapsedMilliseconds} ms.");
        }

        private async UniTask<ReleaseInfo> LoadFromFile(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                await _semaphoreSlim.WaitAsync(cancellationToken);
                var data = await _fileHelper.LoadFromFileAsync<ReleaseInfo>(filePath, cancellationToken);
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

        private async UniTask SaveItem(ReleaseInfo item)
        {
            var filePath = _pathHelper.GetFilePathOf(item);
            _logger.LogInfo($"Create new file {filePath}");
            await _fileHelper.SaveToFileAsync(filePath, item, cancellationToken: default);
        }

        private async UniTask DeleteItem(ReleaseInfo item)
        {
            var filePath = _pathHelper.GetFilePathOf(item);
            _logger.LogInfo($"Delete file at {filePath}");
            await _fileHelper.RemoveFile(filePath, default);
            await _fileHelper.RemoveFile(System.IO.Path.ChangeExtension(filePath, "dt"), default);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _semaphoreSlim?.Dispose();

            UnityEngine.Debug.Log("Data container dispose");
        }
    }
}