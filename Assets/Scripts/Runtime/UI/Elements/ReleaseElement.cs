using System;
using R3;
using Runtime.Core;
using Runtime.Interfaces.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Elements
{
    public class ReleaseElement : DestroyableObject, IClickableBindable<DataClass>
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text textElement;

        private DataClass _dataClass;
        private readonly SerialDisposable _clickDisposable = new SerialDisposable();
        protected override IDisposable Disposable => _clickDisposable;

        public void Bind(DataClass data)
        {
            if (textElement)
                textElement.text = $"#{data.SomeId}";

            _dataClass = data;
            _clickDisposable.Disposable = null;
        }

        public void AddOnClickListener(Action<DataClass> onClick)
        {
            _clickDisposable.Disposable = button.OnClickAsObservable()
                .TakeUntil(destroyCancellationToken)
                .Subscribe(_ => onClick?.Invoke(_dataClass));
        }
        
        
    }
}