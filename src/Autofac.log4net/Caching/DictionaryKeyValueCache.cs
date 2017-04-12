using System.Collections;
using System.Collections.Generic;

namespace Autofac.log4net.Caching
{
    public class DictionaryKeyValueCache<TKey, TValue> : IKeyValueCache<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _cacheDictionary;

        public DictionaryKeyValueCache()
        {
            _cacheDictionary = new Dictionary<TKey, TValue>();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _cacheDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_cacheDictionary).GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _cacheDictionary.Add(item);
        }

        public void Clear()
        {
            _cacheDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _cacheDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _cacheDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _cacheDictionary.Remove(item);
        }

        public int Count
        {
            get { return _cacheDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _cacheDictionary.IsReadOnly; }
        }

        public void AddEntry(TKey key, TValue value)
        {
            _cacheDictionary[key] = value;
        }

        public bool ContainsKey(TKey key)
        {
            return _cacheDictionary.ContainsKey(key);
        }

        public TValue GetEntryValue(TKey key)
        {
            return _cacheDictionary[key];
        }
    }
}