using System;
using System.Collections.Generic;

namespace Autofac.log4net.Mapping
{
    public class DictionaryTypeLoggerMapper : ITypeLoggerMapper
    {
        private readonly IDictionary<Type, string> _typesToLoggers;

        public DictionaryTypeLoggerMapper() : this(new Dictionary<Type, string>())
        {
        }

        public DictionaryTypeLoggerMapper(IDictionary<Type, string> typesToLoggers)
        {
            _typesToLoggers = typesToLoggers;
        }

        public void MapTypeToLoggerName(Type type, string loggerName)
        {
            _typesToLoggers[type] = loggerName;
        }

        public string GetLoggerName(Type type)
        {
            var loggerName = _typesToLoggers.ContainsKey(type) ? _typesToLoggers[type] : type.ToString();
            return loggerName;
        }
    }
}