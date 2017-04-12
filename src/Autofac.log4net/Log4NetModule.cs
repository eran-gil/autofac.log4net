using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using Autofac.log4net.log4net;
using Autofac.log4net.Mapping;
using log4net;

namespace Autofac.log4net
{
    /// <summary>
    /// This module adds the ability to integrate the log4net framework with Autofac, together with some unique features.
    /// </summary>
    public class Log4NetModule : Module
    {
        private readonly ILog4NetAdapter _log4NetAdapter;
        private readonly ILoggerMapper _loggerMapper;
        private const BindingFlags RelevantProeprties = BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        /// Name of the configuration file to be used for the logger definition.
        /// Default value is null;
        /// </summary>
        public string ConfigFileName { get; set; }

        /// <summary>
        /// This flag indicates whether the log4net configuration file should be watched for changes.
        /// Default value is true.
        /// </summary>
        public bool ShouldWatchConfiguration { get; set; }

        /// <summary>
        /// Default Constructor for the Log4NetModule.
        /// Creates the module with the default implementations for its dependencies.
        /// </summary>
        public Log4NetModule() :
            this(new Log4NetAdapter(), new CachedDictionaryLoggerMapper())
        {
        }

        /// <summary>
        /// This constructor is mainly used for dependency injection in testing.
        /// Should not be used unless necessary to override the default implementations.
        /// </summary>
        /// <param name="log4NetAdapter">Adapter to the log4net framework</param>
        /// <param name="loggerMapper">Mapper for logger names in order to allow easier injection of loggers</param>
        public Log4NetModule(ILog4NetAdapter log4NetAdapter, ILoggerMapper loggerMapper)
        {
            _log4NetAdapter = log4NetAdapter;
            _loggerMapper = loggerMapper;
            ConfigFileName = null;
            ShouldWatchConfiguration = true;
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

        /// <inheritdoc />
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;

            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            _log4NetAdapter.ConfigureAndWatch(ConfigFileName, ShouldWatchConfiguration);
            base.Load(builder);
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

        private void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Union(
                new[]
                {
                    new ResolvedParameter(
                        (p, i) => p.ParameterType == typeof(ILog),
                        (p, i) => GetLoggerFromType(p.Member.DeclaringType)
                    ),
                });
        }

        private ILog GetLoggerFromType(Type type)
        {
            var loggerName = _loggerMapper.GetLoggerName(type);
            var logger = _log4NetAdapter.GetLogger(loggerName);
            return logger;
        }

        private static IEnumerable<PropertyInfo> GetILogProperties(IReflect instanceType)
        {
            var instanceProperties = instanceType.GetProperties(RelevantProeprties);
            var writableLogProperties = instanceProperties.Where(IsWritableLogProperty);
            return writableLogProperties;
        }

        private static bool IsWritableLogProperty(PropertyInfo p)
        {
            return p.PropertyType == typeof(ILog) && p.CanWrite && p.GetIndexParameters().Length == 0;
        }
    }
}
