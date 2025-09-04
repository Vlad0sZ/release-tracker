using System.Collections.Generic;
using Runtime.Models;

namespace Runtime.Interfaces.Containers
{
    /// <summary>
    /// Container for <see cref="ReleaseInfo"/>.
    /// </summary>
    public interface IDataContainer
    {
        /// <summary>
        /// All loaded <see cref="ReleaseInfo"/> from previous state.
        /// </summary>
        public IList<ReleaseInfo> Data { get; }

        /// <summary>
        /// Save <see cref="ReleaseInfo"/> to state.
        /// </summary>
        /// <param name="releaseInfo">Exists in Data ReleaseInfo.</param>
        public void Update(ReleaseInfo releaseInfo);
    }
}