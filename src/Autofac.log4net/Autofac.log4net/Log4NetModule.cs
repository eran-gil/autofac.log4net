using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using log4net;
using log4net.Config;

namespace Autofac.log4net
{
    public class Log4NetModule : Module
    {
        private readonly IDictionary<string, string> _typesToLoggers;

        public string ConfigFileName { get; set; }

        public bool ShouldWatchConfiguration { get; set; }

        public Log4NetModule() : this(new Dictionary<string, string>())
        {
        }

        public Log4NetModule(IDictionary<string, string> typesToLoggers)
        {
            _typesToLoggers = typesToLoggers;
            ConfigFileName = null;
            ShouldWatchConfiguration = true;
        }

        public void MapTypeToLoggerName(string type, string loggerName)
        {
            _typesToLoggers[type] = loggerName;
        }

        private void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            // Get all the injectable properties to set.
            // If you wanted to ensure the properties were only UNSET properties,
            // here's where you'd do it.
            var properties = instanceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(ILog) && p.CanWrite && p.GetIndexParameters().Length == 0);

            var logger = GetLoggerFromType(instanceType);
            // Set the properties located.
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
            var typeString = type.ToString();
            var loggerName = _typesToLoggers.ContainsKey(typeString) ? _typesToLoggers[typeString] : typeString;
            var logger = LogManager.GetLogger(loggerName);
            return logger;

        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            // Handle constructor parameters.
            registration.Preparing += OnComponentPreparing;

            // Handle properties.
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (ConfigFileName != null)
            {
                var configFileInfo = new FileInfo(ConfigFileName);
                if (ShouldWatchConfiguration)
                {
                    XmlConfigurator.ConfigureAndWatch(configFileInfo);
                }
                else
                {
                    XmlConfigurator.Configure(configFileInfo);
                }
            }
            base.Load(builder);
        }
    }
}
