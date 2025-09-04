using System.IO;
using JetBrains.Annotations;
using Runtime.Interfaces.IO;
using UnityEngine;

namespace Runtime.IO
{
    /// <summary>
    /// Path helper implementation.
    /// Uses <see cref="Application.persistentDataPath"/> for path.
    /// </summary>
    [UsedImplicitly]
    public sealed class PathHelper : IPathHelper
    {
        public string GetDataPath()
        {
            var dataPath = Path.Combine(Application.persistentDataPath, "Data");

            if (Directory.Exists(dataPath) == false)
                Directory.CreateDirectory(dataPath);

            return dataPath;
        }

        public string GetFilePathOf<T>(T data) where T : IPathable
        {
            var directory = GetDataPath();
            var fileName = $"{data.FileName}.{data.Extension}";

            return Path.Combine(directory, fileName);
        }
    }
}