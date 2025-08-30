using JetBrains.Annotations;
using Runtime.Interfaces.Services;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Views
{
    [UsedImplicitly]
    public sealed class LoadingPage : BaseNavigationScreen
    {
        public LoadingPage(ITemplateLoader templateLoader) : base(templateLoader)
        {
            InitializeComponents();
        }

        protected override void OnComponentInitialized()
        {
            this.StretchToParentSize();
        }
    }
}