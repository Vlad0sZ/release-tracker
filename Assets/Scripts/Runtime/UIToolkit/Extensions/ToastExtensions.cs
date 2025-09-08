using Unity.AppUI.UI;
using UnityEngine.UIElements;

namespace Runtime.UIToolkit.Extensions
{
    public static class ToastExtensions
    {
        public static Toast SetVariables(this Toast toast, params object[] variables)
        {
            var localizedTextElement = toast.view.Q<LocalizedTextElement>("appui-toast__message");
            localizedTextElement.variables = variables;
            return toast;
        }
    }
}