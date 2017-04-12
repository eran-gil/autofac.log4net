using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.log4net.Caching;

namespace Autofac.log4net.Mapping
{
    public class CachedDictionaryLoggerMapper : ILoggerMapper
    {
        private readonly IDictionary<Type, string> _typesToLoggers;
        private readonly IDictionary<string, string> _namespacesToLoggers;
        private readonly IKeyValueCache<Type, string> _typesToLoggersCache;

        public CachedDictionaryLoggerMapper() :
            this(new Dictionary<Type, string>(), new SortedDictionary<string, string>(), new DictionaryKeyValueCache<Type, string>())
        {
        }

        public CachedDictionaryLoggerMapper(IDictionary<Type, string> typesToLoggers, IDictionary<string, string> namespacesToLoggers, IKeyValueCache<Type, string> typesToLoggersCache)
        {
            _typesToLoggers = typesToLoggers;
            _typesToLoggersCache = typesToLoggersCache;
            _namespacesToLoggers = new SortedDictionary<string, string>(namespacesToLoggers, new NamespaceLengthComparer());
        }

        public void MapTypeToLoggerName(Type type, string loggerName)
        {
            _typesToLoggers[type] = loggerName;
            _typesToLoggersCache.Clear();
        }

        public void MapNamespaceToLoggerName(string @namespace, string loggerName)
        {
            _namespacesToLoggers[@namespace] = loggerName;
            _typesToLoggersCache.Clear();
        }

        public string GetLoggerName(Type type)
        {
            if (_typesToLoggersCache.ContainsKey(type))
            {
                return _typesToLoggersCache.GetEntryValue(type);
            }

            var loggerName = type.ToString();
            if (_typesToLoggers.ContainsKey(type))
            {
                loggerName = _typesToLoggers[type];
            }

            var matchingNamespaces = _namespacesToLoggers.Keys.Where(type.IsInNamespace).ToList();
            if (matchingNamespaces.Any())
            {
                var matchingNameSpace = matchingNamespaces.First();
                loggerName = _namespacesToLoggers[matchingNameSpace];
            }
            _typesToLoggersCache.AddEntry(type, loggerName);
            return loggerName;
        }
    }
}