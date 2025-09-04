using Runtime.Interfaces.Services;
using Runtime.Interfaces.UI;
using Unity.AppUI.Navigation;
using Unity.AppUI.UI;
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
            var content = root.contentContainer.GetChildren<VisualElement>(false);

            foreach (var element in content)
                Add(element);

            InitializeComponent();
        }
    }
}