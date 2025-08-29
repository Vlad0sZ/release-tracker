using System;
using R3;
using UnityEngine;

namespace Runtime.Core
{
    public abstract class DestroyableObject : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        protected virtual IDisposable Disposable => _disposables;

        protected virtual void OnDestroy() =>
            Disposable.Dispose();
    }
}