using System.Collections;
using System.Collections.Generic;

namespace Autofac.log4net.Caching
{
    /// <summary>
    /// This class implements the <see cref="IKeyValueCache{TKey,TValue}"/>.
    /// It uses a dictionary for storing the cached values./>
    /// </summary>
    /// <typeparam name="TKey">Type of the key in each cache entry</typeparam>
    /// <typeparam name="TValue">Type of the value in each cache entry</typeparam>
    public class DictionaryKeyValueCache<TKey, TValue> : IKeyValueCache<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _cacheDictionary;

        /// <summary>
        /// 
        /// </summary>
        public DictionaryKeyValueCache()
        {
            _cacheDictionary = new Dictionary<TKey, TValue>();
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _cacheDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_cacheDictionary).GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _cacheDictionary.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _cacheDictionary.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _cacheDictionary.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _cacheDictionary.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _cacheDictionary.Remove(item);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _cacheDictionary.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return _cacheDictionary.IsReadOnly; }
        }

        /// <inheritdoc />
        public void AddEntry(TKey key, TValue value)
        {
            _cacheDictionary[key] = value;
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            return _cacheDictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public TValue GetEntryValue(TKey key)
        {
            return _cacheDictionary[key];
        }
    }
}