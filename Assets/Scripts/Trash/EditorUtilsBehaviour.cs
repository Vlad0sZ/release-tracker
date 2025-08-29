using System.IO;
using Runtime.Core;
using UnityEngine;

namespace Trash
{
    public class EditorUtilsBehaviour : MonoBehaviour
    {
        [ContextMenu(nameof(Create100Files))]
        public void Create100Files()
        {
            var path = Application.persistentDataPath + "/Data";
            var dataClass = new DataClass();

            for (int i = 0; i < 100; i++)
            {
                dataClass.SomeId = i;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(dataClass);
                var filePath = Path.Combine(path, $"data{i}.json");
                File.WriteAllText(filePath, json);
            }
        }
    }
}