using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Models
{
    [System.Serializable]
    public sealed class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        [System.Serializable]
        private class SerializableValues
        {
            public TKey key;

            public TValue value;
        }

        [SerializeField] private SerializableValues[] valuesArray;

        private Dictionary<TKey, TValue> _innerDictionary;

        private Dictionary<TKey, TValue> InnerDictionary
        {
            get
            {
                if (_innerDictionary == null || _innerDictionary.Count != valuesArray.Length)
                    _innerDictionary = valuesArray.ToDictionary(k => k.key, v => v.value);

                return _innerDictionary;
            }
        }

        public int Count => InnerDictionary.Count;

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get => InnerDictionary[key];
            set => InnerDictionary[key] = value;
        }

        public ICollection<TKey> Keys => InnerDictionary.Keys;

        public ICollection<TValue> Values => InnerDictionary.Values;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
            InnerDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item) =>
            InnerDictionary.Add(item.Key, item.Value);

        public void Clear() =>
            InnerDictionary.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) =>
            ((ICollection<KeyValuePair<TKey, TValue>>) InnerDictionary).Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<TKey, TValue>>) InnerDictionary).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) =>
            InnerDictionary.Remove(item.Key);

        public void Add(TKey key, TValue value) =>
            InnerDictionary.Add(key, value);

        public bool ContainsKey(TKey key) =>
            InnerDictionary.ContainsKey(key);

        public bool Remove(TKey key) =>
            InnerDictionary.Remove(key);

        public bool TryGetValue(TKey key, out TValue value) =>
            InnerDictionary.TryGetValue(key, out value);
    }
}