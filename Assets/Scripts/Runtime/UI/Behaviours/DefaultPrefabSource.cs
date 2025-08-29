using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Behaviours
{
    public sealed class DefaultPrefabSource : MonoBehaviour, LoopScrollPrefabSource
    {
        [SerializeField] private GameObject gameObjectPrefab;

        private readonly Stack<Transform> _pool = new Stack<Transform>(128);

        public GameObject GetObject(int index)
        {
            if (_pool.Count == 0)
            {
                return Instantiate(gameObjectPrefab);
            }

            var candidate = _pool.Pop();
            var obj = candidate.gameObject;
            obj.SetActive(true);
            return obj;
        }

        public void ReturnObject(Transform trans)
        {
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            _pool.Push(trans);
        }
    }
}