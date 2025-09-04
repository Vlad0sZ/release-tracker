using System.ComponentModel;
using Runtime.UIToolkit.ViewModels;
using Unity.AppUI.Navigation;
using Unity.AppUI.UI;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Views
{
    public class TableScreen : BaseNavigationScreen<TableScreenViewModel>
    {
        private GridView _gridView;

        public TableScreen(TableScreenViewModel bindingContext) : base(bindingContext) =>
            BindingContext.PropertyChanged += OnPropertyChanged;

        protected override void InitializeComponent()
        {
            _gridView = this.Q<GridView>("grid-view");
            base.InitializeComponent();
        }


        public override void OnEnter(NavController controller, NavDestination destination, Argument[] args)
        {
            if (args.Length > 0 && args[0].name == "id")
                BindingContext.ReleaseId = args[0].value;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}