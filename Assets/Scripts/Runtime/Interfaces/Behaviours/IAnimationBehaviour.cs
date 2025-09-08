using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Models;

namespace Runtime.Interfaces.Behaviours
{
    public interface IAnimationBehaviour
    {
        UniTask PlayAnimation(ReleaseInfo releaseInfo, CancellationToken cancellationToken);
    }
}