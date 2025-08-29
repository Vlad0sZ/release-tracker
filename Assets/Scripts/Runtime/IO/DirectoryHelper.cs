using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Runtime.Interfaces.IO;

namespace Runtime.IO
{
    [UsedImplicitly]
    internal class DirectoryHelper : IDirectoryHelper
    {
        public IEnumerable<FileInfo> GetAllFiles(string directoryPath, [CanBeNull] string searchPattern = null)
        {
            var dirInfo = new DirectoryInfo(directoryPath);

            if (dirInfo.Exists == false)
                return Array.Empty<FileInfo>();

            return dirInfo.GetFiles(searchPattern ?? string.Empty, SearchOption.AllDirectories)
                .ToArray();
        }
    }
}