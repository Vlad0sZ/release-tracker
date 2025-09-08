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
using Button = Unity.AppUI.UI.Button;

namespace Runtime.UIToolkit.Views
{
    public class TableScreen : BaseNavigationScreen<TableScreenViewModel>
    {
        private readonly VisualTreeAsset _rowTemplate;
        private GridView _gridView;
        private ProgressButton _progressButton;

        public TableScreen(TableScreenViewModel bindingContext, ITemplateLoader templateLoader) : base(bindingContext)
        {
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

            _progressButton = new ProgressButton()
            {
                title = "@UI:table.button.text",
                tooltip = "@UI:table.button.tooltip"
            };

            this.Q<VisualElement>("root").Add(_progressButton);
            _progressButton.clickable.command = BindingContext.ShowAnimationCommand;
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
            else if (e.PropertyName == nameof(BindingContext.IsLoading))
            {
                _progressButton.IsProgress = BindingContext.IsLoading;
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

            row.IsReadOnly = !IsCanEditRow(releaseTable, i);

            var disposable = row.OnChanged
                .Subscribe(OnValueChanged);

            row.userData = disposable;
        }

        private static void UnbindItem(VisualElement element, int i) =>
            element.Unsubscribe();

        private void OnValueChanged(ReleaseDataRow row)
        {
            BindingContext.SaveCommand.ExecuteAsync(row);
            _gridView.itemsSource = BindingContext.Release.Table;
        }


        private static bool IsCanEditRow(ReleaseDataRow[] rows, int i)
        {
            bool isNextIsZero = rows.Skip(i + 1).All(x => x.Fact == 0);

            if (i == 0)
                return isNextIsZero;

            bool isPreviousMoreZero = rows.Take(i).All(x => x.Fact > 0);
            return isPreviousMoreZero && isNextIsZero;
        }
    }
}