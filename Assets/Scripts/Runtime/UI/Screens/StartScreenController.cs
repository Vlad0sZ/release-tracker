using System.Collections.Generic;
using R3;
using Runtime.Core;
using Runtime.Interfaces.Containers;
using Runtime.Interfaces.Logging;
using Runtime.Interfaces.UI;
using Runtime.UI.Behaviours;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Runtime.UI.Screens
{
    public class StartScreenController : NavigationScreenBase, LoopScrollDataSource
    {
        [SerializeField] private LoopScrollRect scrollRect;
        [SerializeField] private Button createNewButton;
        [SerializeField] private DefaultPrefabSource prefabSource;


        private IDataContainer _dataContainer;
        private ILogger<StartScreenController> _logger;

        [Inject]
        public void Construct(IDataContainer dataContainer, ILogger<StartScreenController> logger)
        {
            _dataContainer = dataContainer;
            _logger = logger;

            _logger.LogInfo($"Inject {this.GetType()}");
        }

        private void Start()
        {
            createNewButton.OnClickAsObservable()
                .Subscribe(OnCreateNewClicked)
                .AddTo(this);

            scrollRect.dataSource = this;
            scrollRect.prefabSource = prefabSource;
        }

        public void ProvideData(Transform dataTransform, int idx)
        {
            if (dataTransform.TryGetComponent<IClickableBindable<DataClass>>(out var component) == false)
                return;

            var data = _dataContainer.Data[idx];
            component.Bind(data);
            component.AddOnClickListener(MoveToNextScreen);
        }

        protected override void AfterShow()
        {
            scrollRect.totalCount = _dataContainer.Data.Count;
            scrollRect.RefillCells();
        }

        private void OnCreateNewClicked(Unit _) =>
            MoveToNextScreen(null);

        private void MoveToNextScreen(DataClass data)
        {
            if (data == null)
            {
                _logger.LogInfo("Create new release");
            }
            else
            {
                _logger.LogInfo($"Open release {data.SomeId}");
            }
        }
    }
}