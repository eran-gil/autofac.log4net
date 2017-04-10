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
    public class Log4NetModule : Module
    {
        private readonly ILog4NetAdapter _log4NetAdapter;
        private readonly ITypeLoggerMapper _typeLoggerMapper;
        private const BindingFlags RelevantProeprties = BindingFlags.Public | BindingFlags.Instance;

        public string ConfigFileName { get; set; }

        public bool ShouldWatchConfiguration { get; set; }

        public Log4NetModule() :
            this(new Log4NetAdapter(), new DictionaryTypeLoggerMapper())
        {
        }

        public Log4NetModule(ILog4NetAdapter log4NetAdapter, ITypeLoggerMapper typeLoggerMapper)
        {
            _log4NetAdapter = log4NetAdapter;
            _typeLoggerMapper = typeLoggerMapper;
            ConfigFileName = null;
            ShouldWatchConfiguration = true;
        }

        public void MapTypeToLoggerName(Type type, string loggerName)
        {
            _typeLoggerMapper.MapTypeToLoggerName(type, loggerName);
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;

            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

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
            var loggerName = _typeLoggerMapper.GetLoggerName(type);
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
