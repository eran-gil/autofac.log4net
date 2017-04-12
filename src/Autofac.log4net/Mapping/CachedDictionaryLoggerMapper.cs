using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.log4net.Caching;

namespace Autofac.log4net.Mapping
{
    /// <summary>
    /// Logger mapper based on dictionaries and a cache.
    /// The cache is used to allow faster creation of multiple instances of the same type.
    /// </summary>
    public class CachedDictionaryLoggerMapper : ILoggerMapper
    {
        private readonly IDictionary<Type, string> _typesToLoggers;
        private readonly IDictionary<string, string> _namespacesToLoggers;
        private readonly IKeyValueCache<Type, string> _typesToLoggersCache;

        /// <summary>
        /// Default constructor for this class.
        /// Initiates the instance with empty dictionaries and default implementations for its dependencies.
        /// </summary>
        public CachedDictionaryLoggerMapper() :
            this(new Dictionary<Type, string>(), new SortedDictionary<string, string>(), new DictionaryKeyValueCache<Type, string>())
        {
        }

        /// <summary>
        /// Constructor with all the mappings available for custom initialization from the constructor.
        /// </summary>
        /// <param name="typesToLoggers">A dictionary that maps types to logger names</param>
        /// <param name="namespacesToLoggers">A dictionary that maps namespaces to logger names</param>
        public CachedDictionaryLoggerMapper(IDictionary<Type, string> typesToLoggers,
            IDictionary<string, string> namespacesToLoggers) :
            this(typesToLoggers, namespacesToLoggers, new DictionaryKeyValueCache<Type, string>())
        {
        }

        /// <summary>
        /// Constructor with all the mappings and dependencies available for custom initialization from the constructor.
        /// Should not be used unless there's a specific need to override the caching mechanism.
        /// </summary>
        /// <param name="typesToLoggers">A dictionary that maps types to logger names</param>
        /// <param name="namespacesToLoggers">A dictionary that maps namespaces to logger names</param>
        /// <param name="typesToLoggersCache">Cache mechanism to allow faster search for known type-logger-mappings</param>
        public CachedDictionaryLoggerMapper(IDictionary<Type, string> typesToLoggers, IDictionary<string, string> namespacesToLoggers, IKeyValueCache<Type, string> typesToLoggersCache)
        {
            _typesToLoggers = typesToLoggers;
            _typesToLoggersCache = typesToLoggersCache;
            _namespacesToLoggers = new SortedDictionary<string, string>(namespacesToLoggers, new NamespaceLengthComparer());
        }

        /// <inheritdoc />
        public void MapTypeToLoggerName(Type type, string loggerName)
        {
            _typesToLoggers[type] = loggerName;
            _typesToLoggersCache.Clear();
        }

        /// <inheritdoc />
        public void MapNamespaceToLoggerName(string @namespace, string loggerName)
        {
            _namespacesToLoggers[@namespace] = loggerName;
            _typesToLoggersCache.Clear();
        }

        /// <inheritdoc />
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