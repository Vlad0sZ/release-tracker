using System.ComponentModel;
using System.Linq;
using R3;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.Services;
using Runtime.Models;
using Runtime.UIToolkit.Elements;
using Runtime.UIToolkit.Extensions;
using Runtime.UIToolkit.ViewModels;
using Unity.AppUI.Navigation;
using Unity.AppUI.Navigation.Generated;
using Unity.AppUI.UI;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Views
{
    public class TableScreen : BaseNavigationScreen<TableScreenViewModel>
    {
        private readonly VisualTreeAsset _rowTemplate;
        private readonly IDataContainer _dataContainer;
        private GridView _gridView;

        public TableScreen(TableScreenViewModel bindingContext, ITemplateLoader templateLoader,
            IDataContainer dataContainer) : base(bindingContext)
        {
            _dataContainer = dataContainer;
            _rowTemplate = templateLoader.GetTemplate(nameof(DataRowElement));
            BindingContext.PropertyChanged += OnPropertyChanged;
        }

        protected override void InitializeComponent()
        {
            _gridView = this.Q<GridView>("grid-view");
            _gridView.selectionType = SelectionType.None;
            _gridView.itemHeight = 40;
            _gridView.makeItem = CreateItem;
            _gridView.bindItem = BindItem;
            _gridView.unbindItem = UnbindItem;

            base.InitializeComponent();
        }


        public override void OnEnter(NavController controller, NavDestination destination, Argument[] args)
        {
            base.OnEnter(controller, destination, args);
            if (args.Length > 0 && args[0].name == Arguments.releaseId)
                BindingContext.ReleaseId = args[0].value;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BindingContext.Release))
            {
                this.appBar.title = BindingContext.Release.Name;
                _gridView.itemsSource = BindingContext.Release.Table;
            }
        }


        private VisualElement CreateItem()
        {
            var item = new DataRowElement(_rowTemplate);
            item.Bind(BindingContext.Release);
            return item;
        }

        private void BindItem(VisualElement element, int i)
        {
            var releaseTable = BindingContext.Release.Table;
            var elementRow = releaseTable.ElementAtOrDefault(i);
            if (elementRow == null || element is not DataRowElement row)
                return;

            row.Bind(elementRow);



            int rowNextIndex = i == 0 ? 1 : i - 1;
            var previousRow = releaseTable.ElementAtOrDefault(rowNextIndex);

            bool thisRowIsReadonly = previousRow is {Fact: 0};
            row.IsReadOnly = thisRowIsReadonly;
            var disposable = row.OnChanged
                .Subscribe(OnValueChanged);

            row.userData = disposable;
        }

        private static void UnbindItem(VisualElement element, int i) =>
            element.Unsubscribe();

        private void OnValueChanged(ReleaseDataRow row)
        {
            _dataContainer.Update(BindingContext.Release);
            _gridView.itemsSource = BindingContext.Release.Table;
        }
    }
}