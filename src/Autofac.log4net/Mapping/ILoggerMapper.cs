using System;

namespace Autofac.log4net.Mapping
{
    /// <summary>
    /// This interface allows mapping loggers to types in order to allow customized injection.
    /// </summary>
    public interface ILoggerMapper
    {
        /// <summary>
        /// Maps a type to a logger name, so when the ILog injection occurs, it will choose the correct logger.
        /// </summary>
        /// <param name="type">Type to be mapped</param>
        /// <param name="loggerName">Logger name to be injected into the mapped type</param>
        void MapTypeToLoggerName(Type type, string loggerName);

        /// <summary>
        /// Maps a namespace to a logger name, so when the ILog injection occurs, it will choose the correct logger.
        /// The module mapper will always return the most specific mapped-namespace's logger.
        /// </summary>
        /// <param name="namespace">Namespace to be mapped</param>
        /// <param name="loggerName">Logger name to be injected into all types in the mapped namespace</param>
        void MapNamespaceToLoggerName(string @namespace, string loggerName);

        /// <summary>
        /// Gets the logger name for a type according to the mappings in the mapper.
        /// </summary>
        /// <param name="type">Type that needs a logger</param>
        /// <returns>If there's a namespace or type mapping for this type, returns the mapped logger. Otherwise, returns the full name of the type as the logger name.</returns>
        string GetLoggerName(Type type);
    }
}
