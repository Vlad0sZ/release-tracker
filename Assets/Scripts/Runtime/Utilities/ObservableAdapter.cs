using System;
using System.Collections;
using JetBrains.Annotations;
using ObservableCollections;

namespace Runtime.Utilities
{
    public class ObservableListAdapter<T> : IList
    {
        private readonly ObservableList<T> _inner;

        public ObservableListAdapter(ObservableList<T> inner) =>
            _inner = inner;

        public int Count => _inner.Count;

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public object SyncRoot => _inner.SyncRoot;

        public bool IsSynchronized => false;

        public object this[int index]
        {
            get => (object) _inner[index];
            set => _inner[index] = Cast(value);
        }

        public int Add(object value)
        {
            _inner.Add(Cast(value));
            return _inner.Count - 1;
        }

        public void Clear() => _inner.Clear();
        public bool Contains(object value) => _inner.Contains(Cast(value));
        public int IndexOf(object value) => _inner.IndexOf(Cast(value));
        public void Insert(int index, object value) => _inner.Insert(index, Cast(value));
        public void Remove(object value) => _inner.Remove(Cast(value));
        public void RemoveAt(int index) => _inner.RemoveAt(index);
        public void CopyTo(Array array, int index) => (_inner as ICollection)?.CopyTo(array, index);

        public IEnumerator GetEnumerator() => _inner.GetEnumerator();


        private T Cast([CanBeNull] object value)
        {
            if (value is not T item)
                throw new InvalidCastException($"can not cast value type {value?.GetType()} to type {typeof(T)}");


            return item;
        }
    }
}