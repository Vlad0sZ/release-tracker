using Unity.AppUI.MVVM;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.AppUI.Extended.DependencyInjection
{
    public abstract class ExtendedAppHost<T> : MonoBehaviour where T : App
    {
        /// <summary>
        /// The UIDocument to host the app in.
        /// </summary>
        [Tooltip("The UIDocument to host the app in.")]
        public UIDocument uiDocument;

        private void OnEnable()
        {
            if (!uiDocument)
            {
                Debug.LogWarning("No UIDocument assigned to Program component. Aborting App startup.");
                return;
            }

            var builder = ExtendedAppBuilder.InstantiateWith<T, UIToolkitHost>();
            OnConfiguringApp(builder);

            var host = new UIToolkitHost(uiDocument);
            var app = (T) builder.BuildWith(host);
            OnAppInitialized(app);
        }

        /// <summary>
        /// Called when the app builder is being configured.
        /// </summary>
        /// <param name="builder"> The app builder. </param>
        protected virtual void OnConfiguringApp(IAppConfiguration builder)
        {
        }

        /// <summary>
        /// Called when the app has been initialized.
        /// </summary>
        /// <param name="app"> The app that was initialized. </param>
        protected virtual void OnAppInitialized(T app)
        {
        }

        /// <summary>
        /// Called when the app is shutting down.
        /// </summary>
        /// <param name="app"> The app that is shutting down. </param>
        protected virtual void OnAppShuttingDown(T app)
        {
        }

        private void OnDisable()
        {
            if (App.current is not T app)
                return;

            OnAppShuttingDown(app);
            if (uiDocument)
                uiDocument.rootVisualElement?.Clear();
            app.Shutdown();
            app.Dispose();
        }
    }
}