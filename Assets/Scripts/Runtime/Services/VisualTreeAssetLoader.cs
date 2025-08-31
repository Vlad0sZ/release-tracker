using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Runtime.Extensions;
using Runtime.Interfaces.Logging;
using Runtime.Interfaces.Services;
using Runtime.UIToolkit;
using UnityEngine.UIElements;

namespace Runtime.Services
{
    [UsedImplicitly]
    internal sealed class VisualTreeAssetLoader : ITemplateLoader
    {
        private readonly Dictionary<string, VisualTreeAsset> _templates;
        private readonly ILogger<ITemplateLoader> _logger;

        public VisualTreeAssetLoader(AssetProvider assetProvider, ILogger<ITemplateLoader> logger)
        {
            _templates = assetProvider.TemplateAssets.ToDictionary(x => x.name);
            _logger = logger;
        }

        public VisualTreeAsset GetTemplate(string treeName)
        {
            if (_templates.TryGetValue(treeName, out var treeAsset))
            {
                _logger.LogInfo($"Find asset with name {treeName}");
                return treeAsset;
            }

            _logger.LogWarning($"Can not find asset with name {treeName}");
            return default;
        }
    }
}