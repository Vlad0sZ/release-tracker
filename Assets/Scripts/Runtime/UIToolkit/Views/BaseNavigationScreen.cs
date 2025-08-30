using Runtime.Interfaces.Services;
using Unity.AppUI.Navigation;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Views
{
    /// <summary>
    /// Base View class for initialize views with UXML files.
    /// </summary>
    public class BaseNavigationScreen : NavigationScreen
    {
        private readonly ITemplateLoader _templateLoader;

        protected BaseNavigationScreen(ITemplateLoader templateLoader) =>
            _templateLoader = templateLoader;

        protected void InitializeComponents()
        {
            var treeAsset = _templateLoader.GetTemplate(this.GetType().Name);
            if (treeAsset != null)
                treeAsset.CloneTree(this);


            OnComponentInitialized();
        }

        protected virtual void OnComponentInitialized()
        {
        }
    }
}