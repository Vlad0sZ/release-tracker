using System.Collections.Generic;
using Runtime.Core;

namespace Runtime.Interfaces.Containers
{
    public interface IDataContainer
    {
        public IList<DataClass> Data { get; }
    }
}