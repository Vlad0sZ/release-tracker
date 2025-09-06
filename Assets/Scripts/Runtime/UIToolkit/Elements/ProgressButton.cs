using System;
using Unity.AppUI.UI;
using UnityEngine.UIElements;
using Button = Unity.AppUI.UI.Button;

namespace Runtime.UIToolkit.Elements
{
    public class ProgressButton : Button
    {
        private readonly VisualElement _titleElement;
        private readonly VisualElement _subTitle;
        private readonly VisualElement _progressElement;
        public const string progressUssClassName = ussClassName + "__progress";

        private bool _progress;

        public bool IsProgress
        {
            get => _progress;
            set => SetProgress(value);
        }

        public ProgressButton() : this(null)
        {
        }

        public ProgressButton(Action clicked) : base(clicked)
        {
            var titleContainer = this.Q<VisualElement>(Button.titleContainerUssClassName);
            _titleElement = titleContainer.Q<LocalizedTextElement>(Button.titleUssClassName);
            _subTitle = titleContainer.Q<LocalizedTextElement>(Button.subtitleUssClassName);

            _progressElement = new CircularProgress()
            {
                name = progressUssClassName,
                style = {display = DisplayStyle.None}
            };

            titleContainer.Add(_progressElement);
        }

        private void SetProgress(bool value)
        {
            if (_progress == value)
                return;

            _progressElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            _titleElement.style.display = value ? DisplayStyle.None : DisplayStyle.Flex;
            _subTitle.style.display = value ? DisplayStyle.None : DisplayStyle.Flex;
            this.SetEnabled(!value);
            _progress = value;
        }
    }
}