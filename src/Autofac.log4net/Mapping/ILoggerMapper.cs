using System;

namespace Autofac.log4net.Mapping
{
    public interface ILoggerMapper
    {
        void MapTypeToLoggerName(Type type, string loggerName);

        void MapNamespaceToLoggerName(string @namespace, string loggerName);

        string GetLoggerName(Type type);
    }
}
