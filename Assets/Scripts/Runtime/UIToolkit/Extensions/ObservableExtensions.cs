using System;
using R3;
using Unity.AppUI.UI;
using UnityEngine.UIElements;
using Button = Unity.AppUI.UI.Button;

namespace Runtime.UIToolkit.Extensions
{
    public static class ObservableExtensions
    {
        public static Observable<Unit> AsObservable(this IPressable pressable)
        {
            return Observable.FromEvent(
                h => pressable.clickable.clicked += h,
                h => pressable.clickable.clicked -= h);
        }

        public static Observable<T> AsObservable<T>(this IPressable pressable, Func<T> conversation)
        {
            return Observable.FromEvent<Action, T>(
                conversion: h => () => h(conversation()),
                h => pressable.clickable.clicked += h,
                h => pressable.clickable.clicked -= h);
        }

        public static void Subscribe(this IPressable pressable, Observer<Unit> action)
        {
            var disposable = Observable.FromEvent(
                    h => pressable.clickable.clicked += h,
                    h => pressable.clickable.clicked -= h)
                .Subscribe(action);

            if (pressable is VisualElement ve)
                ve.userData = disposable;
        }

        public static void Unsubscribe(this VisualElement visualElement)
        {
            if (visualElement.userData is IDisposable disposable)
                disposable.Dispose();

            visualElement.userData = null;
        }
    }
}