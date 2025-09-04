using System;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using R3;
using Runtime.Interfaces.Models;
using Runtime.Interfaces.Services;
using Runtime.Models;
using Runtime.Types;
using Runtime.UIToolkit.Elements;
using Runtime.UIToolkit.Extensions;
using Runtime.UIToolkit.ViewModels;
using Unity.AppUI.Navigation;
using UnityEngine.UIElements;
using UIActionButton = Unity.AppUI.UI.ActionButton;

namespace Runtime.UIToolkit.Views
{
    [UsedImplicitly]
    public sealed class StartScreen : BaseNavigationScreen<StartScreenViewModel>
    {
        private ListView _listView;
        private Label _emptyLabel;
        private UIActionButton _createButton;

        private readonly VisualTreeAsset _dataItemTemplate;

        public StartScreen(ITemplateLoader templateLoader, StartScreenViewModel bindingContext) :
            base(bindingContext) =>
            _dataItemTemplate = templateLoader.GetTemplate(nameof(DataItemElement));


        protected override void InitializeComponent()
        {
            _listView = this.Q<ListView>("previousDataScroll");
            _emptyLabel = this.Q<Label>("emptyListLabel");
            _createButton = this.Q<UIActionButton>("createNewButton");
            _createButton.clicked += BindingContext.CreateCommand.Execute;
            BindingContext.PropertyChanged += OnPropertyChanged;
            InitializeListView();

            base.InitializeComponent();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BindingContext.Data))
                RebuildListView();
        }

        public override void OnEnter(NavController controller, NavDestination destination, Argument[] args) =>
            RebuildListView();

        private void RebuildListView()
        {
            _listView.itemsSource = BindingContext.Data;

            bool isEmpty = BindingContext.Data.Length == 0;

            _emptyLabel.style.display = isEmpty ? DisplayStyle.Flex : DisplayStyle.None;
            _listView.style.display = isEmpty ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private void InitializeListView()
        {
            _listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            _listView.selectionType = SelectionType.None;

            _listView.makeItem = () => new DataItemElement(_dataItemTemplate);

            _listView.bindItem = (elem, idx) =>
            {
                var data = BindingContext?.Data.ElementAtOrDefault(idx);
                if (data == null || elem is not DataItemElement dataItem)
                    return;

                dataItem.Bind(data);
                var disposable = dataItem.OnRequest.Subscribe(OnRequest);
                dataItem.userData = disposable;
            };

            _listView.unbindItem = (elem, _) => elem.Unsubscribe();
            _listView.destroyItem += (elem) => (elem as IDisposable)?.Dispose();
        }


        private void OnRequest(IRequestData<ReleaseInfo> requestData)
        {
            var data = requestData.Value;

            if (requestData.Type == RequestType.Select)
                BindingContext.OpenCommand.Execute(data);
            else if (requestData.Type == RequestType.Delete)
            {
                // TODO show modal window
                BindingContext.DeleteCommand.Execute(data);
            }
        }
    }
}