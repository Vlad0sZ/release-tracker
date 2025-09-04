using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Extensions
{
    public static class BindingExtensions
    {
        public static void SetBinding<TElement>(this TElement element,
            BindingMode bindingMode,
            PropertyPath propertyPath)
            where TElement : VisualElement
        {
            if (TypeToBinding.TryGetValue(typeof(TElement), out var bindingName))
            {
                element.SetBinding(bindingName, new DataBinding()
                {
                    bindingMode = bindingMode,
                    dataSourcePath = propertyPath
                });
            }
            else
            {
                UnityEngine.Debug.LogWarning($"No one binding for {typeof(TextField)}");
            }
        }


        private static readonly Dictionary<Type, string> TypeToBinding = new()
        {
            [typeof(Unity.AppUI.UI.TextField)] = nameof(Unity.AppUI.UI.TextField.value),
            [typeof(Unity.AppUI.UI.Text)] = nameof(Unity.AppUI.UI.Text.text),
            [typeof(Unity.AppUI.UI.InputLabel)] = nameof(Unity.AppUI.UI.InputLabel.label),
            [typeof(Unity.AppUI.UI.IntField)] = nameof(Unity.AppUI.UI.IntField.value),
            [typeof(Unity.AppUI.UI.DateRangeField)] = nameof(Unity.AppUI.UI.DateRangeField.value),
            [typeof(Unity.AppUI.UI.Dropdown)] = nameof(Unity.AppUI.UI.Dropdown.selectedIndex)
        };
    }
}