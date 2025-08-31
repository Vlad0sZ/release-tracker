using JetBrains.Annotations;
using Runtime.UIToolkit.ViewModels;
using Unity.AppUI.Navigation;
using Unity.AppUI.UI;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Views
{
    [UsedImplicitly]
    public sealed class LoadingScreen : BaseNavigationScreen<LoadingPageViewModel>
    {
        public LoadingScreen(LoadingPageViewModel viewModel)
            : base(viewModel)
        {
            var preloader = new Preloader();
            preloader.StretchToParentSize();

            hierarchy.Add(preloader);
        }

        public override void OnEnter(NavController controller, NavDestination destination, Argument[] args)
        {
            base.OnEnter(controller, destination, args);
            BindingContext.InitializeCommand.ExecuteAsync(null);
        }

        public override void ApplyTemplate(VisualTreeAsset template)
        {
            // ignored.
        }
    }
}