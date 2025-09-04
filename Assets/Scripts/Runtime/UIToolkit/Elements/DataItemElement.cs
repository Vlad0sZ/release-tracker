using System;
using JetBrains.Annotations;
using R3;
using Runtime.Interfaces.Models;
using Runtime.Interfaces.UI;
using Runtime.Models;
using Runtime.UIToolkit.Extensions;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Elements
{
    public sealed class DataItemElement : VisualElement, IBindable<ReleaseInfo>, IDisposable
    {
        private readonly Unity.AppUI.UI.Button _mainButton;

        [CanBeNull] private ReleaseInfo _data;

        public readonly Subject<IRequestData<ReleaseInfo>> OnRequest = new();
        private readonly CompositeDisposable _disposable = new();

        public DataItemElement(VisualTreeAsset template)
        {
            if (template != null)
                template.CloneTree(this);

            _mainButton = this.Q<Unity.AppUI.UI.Button>("select");

            var deleteButton = this.Q<Unity.AppUI.UI.IconButton>("delete");

            _mainButton
                .AsObservable<ReleaseInfo>(() => _data)
                .Where(x => x != null)
                .Subscribe(SubscribeToSelect)
                .AddTo(_disposable);


            deleteButton
                .AsObservable(() => _data)
                .Where(x => x != null)
                .Subscribe(SubscribeToDelete)
                .AddTo(_disposable);
        }


        public void Bind(ReleaseInfo data)
        {
            _mainButton.title = data.Name;
            _data = data;
        }

        private void SubscribeToSelect(ReleaseInfo data) =>
            OnRequest.OnNext(new SelectOperation<ReleaseInfo>(data));


        private void SubscribeToDelete(ReleaseInfo data) =>
            OnRequest.OnNext(new DeleteOperation<ReleaseInfo>(data));


        public void Dispose()
        {
            _disposable.Dispose();

            OnRequest.OnCompleted();
            OnRequest.Dispose();
        }
    }
}