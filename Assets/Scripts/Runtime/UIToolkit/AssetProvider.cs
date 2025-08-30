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
    public static class AssetProvider
    {
        public static IList<VisualTreeAsset> TemplateAssets { get; set; }
        public static NavGraphViewAsset NavGraph { get; set; }
    }
}