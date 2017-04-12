using System.Collections.Generic;

namespace Autofac.log4net.Caching
{
    public interface IKeyValueCache<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
    {
        void AddEntry(TKey key, TValue value);

        bool ContainsKey(TKey key);

        TValue GetEntryValue(TKey key);
    }
}
