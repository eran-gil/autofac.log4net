using System.Collections.Generic;

namespace Autofac.log4net.Caching
{
    public interface IKeyValueCache<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
    {
        IEnumerable<TKey> Keys { get; }

        IEnumerable<TValue> Values { get; }

        void AddEntry(TKey key, TValue value);

        TValue GetEntryValue(TKey key);
    }
}
