using System;
using Autofac.Core.Resolving.Pipeline;

namespace Autofac.log4net
{
    public interface ILog4NetMiddleware : IResolveMiddleware
    {
        void Initialize();

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
    }
}