using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.log4net.Mapping
{
    public class DictionaryLoggerMapper : ILoggerMapper
    {
        private readonly IDictionary<Type, string> _typesToLoggers;
        private readonly IDictionary<string, string> _namespacesToLoggers;

        public DictionaryLoggerMapper() :
            this(new Dictionary<Type, string>(), new SortedDictionary<string, string>())
        {
        }

        public DictionaryLoggerMapper(IDictionary<Type, string> typesToLoggers, IDictionary<string, string> namespacesToLoggers)
        {
            _typesToLoggers = typesToLoggers;
            _namespacesToLoggers = new SortedDictionary<string, string>(namespacesToLoggers, new NamespaceLengthComparer());
        }

        public void MapTypeToLoggerName(Type type, string loggerName)
        {
            _typesToLoggers[type] = loggerName;
        }

        public void MapNamespaceToLoggerName(string @namespace, string loggerName)
        {
            _namespacesToLoggers[@namespace] = loggerName;
        }

        public string GetLoggerName(Type type)
        {
            if (_typesToLoggers.ContainsKey(type))
            {
                return _typesToLoggers[type];
            }

            var matchingNamespaces = _namespacesToLoggers.Keys.Where(type.IsInNamespace).ToList();
            if (matchingNamespaces.Any())
            {
                var matchingNameSpace = matchingNamespaces.First();
                return _namespacesToLoggers[matchingNameSpace];
            }

            return type.ToString();
        }
    }
}