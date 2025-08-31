using System.Collections.Generic;
using Unity.AppUI.Navigation;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit
{
    /// <summary>
    /// A way to pass data from the inspector to the constructor.
    /// <remarks>
    ///  TODO remove this when it will be possible to add instance in <see cref="Unity.AppUI.MVVM.IServiceCollection"/>
    /// </remarks>
    /// </summary>
    public sealed class AssetProvider
    {
        public IList<VisualTreeAsset> TemplateAssets { get; set; }
        public NavGraphViewAsset NavGraph { get; set; }

        public AssetProvider(IList<VisualTreeAsset> templateAssets, NavGraphViewAsset navGraph)
        {
            TemplateAssets = templateAssets;
            NavGraph = navGraph;
        }
    }
}