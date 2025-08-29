using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace Runtime.Interfaces.IO
{
    public interface IDirectoryHelper
    {
        IEnumerable<FileInfo> GetAllFiles(string directoryPath, [CanBeNull] string searchPattern = null);
    }
}