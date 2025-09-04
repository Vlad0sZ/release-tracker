using System.Collections.Generic;
using Runtime.Models;

namespace Runtime.Interfaces.Containers
{
    public interface IDataContainer
    {
        public IList<ReleaseInfo> Data { get; }
    }
}