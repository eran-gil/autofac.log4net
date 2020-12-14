using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using Autofac.log4net.log4net;
using Autofac.log4net.Mapping;
using log4net;

namespace Autofac.log4net
{
    /// <summary>
    /// This module adds the ability to integrate the log4net framework with Autofac, together with some unique features.
    /// </summary>
    public class Log4NetMiddleware : ILog4NetMiddleware
    {
        private readonly ILog4NetAdapter _log4NetAdapter;
        private readonly ILoggerMapper _loggerMapper;

        private const BindingFlags RelevantProperties = BindingFlags.Public | BindingFlags.Instance;

        private readonly string _configFileName;
        
        private readonly bool _shouldWatchConfiguration;

        /// <inheritdoc />
        public PipelinePhase Phase => PipelinePhase.ParameterSelection;

        /// <summary>
        /// Default Constructor for the Log4NetMiddleware.
        /// Creates the middleware with the default implementations for its dependencies.
        /// <param name="configFileName">The filename for the log4net config file</param>
        /// <param name="shouldWatchConfiguration">Enables watching for configuration changes in the file</param>
        /// </summary>
        public Log4NetMiddleware(string configFileName = null, bool shouldWatchConfiguration = true) :
            this(new Log4NetAdapter(), new CachedDictionaryLoggerMapper(), configFileName, shouldWatchConfiguration)
        {
        }

        /// <summary>
        /// This constructor is mainly used for dependency injection in testing.
        /// Should not be used unless necessary to override the default implementations.
        /// </summary>
        /// <param name="log4NetAdapter">Adapter to the log4net framework</param>
        /// <param name="loggerMapper">Mapper for logger names in order to allow easier injection of loggers</param>
        /// <param name="configFileName">The filename for the log4net config file</param>
        /// <param name="shouldWatchConfiguration">Enables watching for configuration changes in the file</param>
        public Log4NetMiddleware(ILog4NetAdapter log4NetAdapter, ILoggerMapper loggerMapper, string configFileName=null, bool shouldWatchConfiguration=true)
        {
            _log4NetAdapter = log4NetAdapter;
            _loggerMapper = loggerMapper;
            _configFileName = configFileName;
            _shouldWatchConfiguration = shouldWatchConfiguration;
        }

        public void Initialize()
        {
            _log4NetAdapter.ConfigureAndWatch(_configFileName, _shouldWatchConfiguration);
        }

        /// <inheritdoc />
        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            ResolveContext(context);
            next(context);
            if (context.NewInstanceActivated)
            {
                InjectLoggerProperties(context.Instance);
            }
        }

        /// <summary>
        /// Maps a type to a logger name, so when the ILog injection occurs, it will choose the correct logger.
        /// </summary>
        /// <param name="type">Type to be mapped</param>
        /// <param name="loggerName">Logger name to be injected into the mapped type</param>
        public void MapTypeToLoggerName(Type type, string loggerName)
        {
            _loggerMapper.MapTypeToLoggerName(type, loggerName);
        }

        /// <summary>
        /// Maps a namespace to a logger name, so when the ILog injection occurs, it will choose the correct logger.
        /// The module mapper will always return the most specific mapped-namespace's logger.
        /// </summary>
        /// <param name="namespace">Namespace to be mapped</param>
        /// <param name="loggerName">Logger name to be injected into all types in the mapped namespace</param>
        public void MapNamespaceToLoggerName(string @namespace, string loggerName)
        {
            _loggerMapper.MapNamespaceToLoggerName(@namespace, loggerName);
        }

        private void ResolveContext(ResolveRequestContext context)
        {
            var newParameters = context.Parameters.Union(
                new[]
                {
                    new ResolvedParameter(
                        (p, i) => p.ParameterType == typeof(ILog),
                        (p, i) => GetLoggerFromType(p.Member.DeclaringType)
                    ),
                });
            context.ChangeParameters(newParameters);
        }

        private void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();
            var properties = GetILogProperties(instanceType);
            var logger = GetLoggerFromType(instanceType);

            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, logger, null);
            }
        }

        private ILog GetLoggerFromType(Type type)
        {
            var loggerName = _loggerMapper.GetLoggerName(type);
            var logger = _log4NetAdapter.GetLogger(loggerName);
            return logger;
        }

        private static IEnumerable<PropertyInfo> GetILogProperties(IReflect instanceType)
        {
            var instanceProperties = instanceType.GetProperties(RelevantProperties);
            var writableLogProperties = instanceProperties.Where(IsWritableLogProperty);
            return writableLogProperties;
        }

        private static bool IsWritableLogProperty(PropertyInfo p)
        {
            return p.PropertyType == typeof(ILog) && p.CanWrite && p.GetIndexParameters().Length == 0;
        }
    }
}
