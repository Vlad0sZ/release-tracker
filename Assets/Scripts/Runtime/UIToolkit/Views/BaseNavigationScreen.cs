using Runtime.Interfaces.Services;
using Runtime.Interfaces.UI;
using Unity.AppUI.Navigation;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Views
{
    /// <summary>
    /// Base View class for initialize views with UXML files.
    /// </summary>
    public abstract class BaseNavigationScreen<TViewModel> : NavigationScreen, ITemplateScreen
        where TViewModel : IViewModel
    {
        protected readonly TViewModel BindingContext;

        protected BaseNavigationScreen(TViewModel bindingContext) =>
            BindingContext = bindingContext;

        protected virtual void InitializeComponent() =>
            this.StretchToParentSize();

        public virtual void ApplyTemplate(VisualTreeAsset template)
        {
            var root = template.Instantiate();
            root.StretchToParentSize();
            hierarchy.Add(root);

            InitializeComponent();
        }
    }
}