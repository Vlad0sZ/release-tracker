using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Models
{
    [System.Serializable]
    public class PrefabOf<T>
        where T : MonoBehaviour
    {
        [SerializeField] private GameObject gameObject;

        public T Instantiate(Transform parent, bool stayWorldPosition = false)
        {
            var obj = Object.Instantiate(gameObject, parent, stayWorldPosition);
            return ReturnComponent(obj);
        }

        public async UniTask<T> InstantiateAsync(Transform parent, bool stayWorldPosition = false,
            CancellationToken cancellationToken = default)
        {
            var instantiateParameters = new InstantiateParameters()
            {
                parent = parent,
                worldSpace = stayWorldPosition,
            };

            var operation = Object.InstantiateAsync(gameObject, instantiateParameters, cancellationToken);
            await operation;

            return ReturnComponent(operation.Result[0]);
        }

        private T ReturnComponent(GameObject createdObject)
        {
            return createdObject.GetComponent<T>();
        }
    }
}