using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Behaviours.AnimationStates
{
    public abstract class AnimationState<TPayload> : IDisposable
    {
        private readonly List<GameObject> _instantiateGameObjects = new();
        public abstract UniTask Animate(TPayload payload, CancellationToken cancellationToken);

        public async UniTask Animate(object parameters, CancellationToken cancellationToken)
        {
            if (parameters is TPayload payload)
                await Animate(payload, cancellationToken);
            else
                throw new InvalidCastException($"object {parameters.GetType()} is not a {typeof(TPayload)} type.");
        }

        protected async UniTask<T> Instantiate<T>(Transform parent, PrefabOf<T> prefab,
            CancellationToken cancellationToken)
            where T : MonoBehaviour
        {
            var obj = await prefab.InstantiateAsync(parent, cancellationToken: cancellationToken);
            _instantiateGameObjects.Add(obj.gameObject);
            return obj;
        }

        public virtual void Dispose()
        {
            foreach (var gameObject in _instantiateGameObjects.Where(gameObject => gameObject))
                Object.Destroy(gameObject);

            _instantiateGameObjects.Clear();
        }
    }
}