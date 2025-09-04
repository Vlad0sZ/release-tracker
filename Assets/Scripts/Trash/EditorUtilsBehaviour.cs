using System.IO;
using Runtime.Core;
using Runtime.Models;
using UnityEngine;

namespace Trash
{
    public class EditorUtilsBehaviour : MonoBehaviour
    {
        [ContextMenu(nameof(Create100Files))]
        public void Create100Files()
        {
            var path = Application.persistentDataPath + "/Data";


            for (int i = 0; i < 100; i++)
            {
                var dataClass = new ReleaseInfo();
                dataClass.Name = i.ToString();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(dataClass);
                var filePath = Path.Combine(path, $"data{i}.json");
                File.WriteAllText(filePath, json);
            }
        }
    }
}