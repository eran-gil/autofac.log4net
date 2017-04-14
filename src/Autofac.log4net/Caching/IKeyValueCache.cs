using System.Collections.Generic;

namespace Autofac.log4net.Caching
{
    /// <summary>
    /// This interface allows caching of every key type to every value.
    /// </summary>
    /// <typeparam name="TKey">Type of the key in each cache entry</typeparam>
    /// <typeparam name="TValue">Type of the value in each cache entry</typeparam>
    public interface IKeyValueCache<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Adds an entry to the cache.
        /// </summary>
        /// <param name="key">The key of the entry</param>
        /// <param name="value">The value of the entry</param>
        void AddEntry(TKey key, TValue value);

        /// <summary>
        /// This method indicates whether a key exists in the cache or not.
        /// </summary>
        /// <param name="key">Key to be checked</param>
        /// <returns>True if the key exists in the cache, false otherwise</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// This method retrieves the cached value for a key.
        /// You should make sure the key exists in the cache before using this method.
        /// If the key doesn't exist in the cache, an exception might be thrown.
        /// </summary>
        /// <param name="key">The key of the value that should be retrieved</param>
        /// <returns>The value cached for the requested key</returns>
        TValue GetEntryValue(TKey key);
    }
}
