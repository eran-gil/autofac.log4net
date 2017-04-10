using System;

namespace Autofac.log4net.Mapping
{
    public interface ITypeLoggerMapper
    {
        void MapTypeToLoggerName(Type type, string loggerName);

        string GetLoggerName(Type type);
    }
}
