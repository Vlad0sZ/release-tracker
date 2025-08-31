using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using Runtime.UIToolkit.ViewModels;
using Unity.AppUI.Navigation;
using Unity.AppUI.Navigation.Generated;
using UnityEngine.UIElements;
using UIButton = Unity.AppUI.UI.Button;
using UIActionButton = Unity.AppUI.UI.ActionButton;

namespace Runtime.UIToolkit.Views
{
    [UsedImplicitly]
    public sealed class StartScreen : BaseNavigationScreen<StartScreenViewModel>
    {
        private ListView _listView;
        private Label _emptyLabel;
        private UIActionButton _createButton;

        public StartScreen(StartScreenViewModel bindingContext) : base(bindingContext)
        {
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            _listView = this.Q<ListView>("previousDataScroll");
            _emptyLabel = this.Q<Label>("emptyListLabel");
            _createButton = this.Q<UIActionButton>("createNewButton");


            _listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

            _listView.makeItem = () => new UIButton();
            _listView.bindItem = (elem, idx) =>
            {
                var data = BindingContext?.Data.ElementAtOrDefault(idx);
                if (data == null)
                    return;

                ((UIButton) elem).title = data.SomeId.ToString();
            };

            _createButton.clicked += OnCreateButtonClicked;
        }

        public override void OnEnter(NavController controller, NavDestination destination, Argument[] args)
        {
            var data = BindingContext.Data;
            _listView.itemsSource = (IList) data;

            bool isEmpty = data.Count == 0;

            _emptyLabel.style.display = isEmpty ? DisplayStyle.Flex : DisplayStyle.None;
            _listView.style.display = isEmpty ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private void OnCreateButtonClicked() =>
            this.FindNavController().Navigate(Actions.start_to_create);
    }
}