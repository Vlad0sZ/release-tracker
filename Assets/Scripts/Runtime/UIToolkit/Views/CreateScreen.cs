using Runtime.UIToolkit.ViewModels;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Views
{
    public class CreateScreen : BaseNavigationScreen<CreateScreenViewModel>
    {
        public CreateScreen(CreateScreenViewModel bindingContext) : base(bindingContext)
        {
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            
            
        }
    }
}