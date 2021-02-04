
using System;
using Autofac.Core;
using Autofac.Core.Registration;

namespace Autofac.log4net
{
    public class Log4NetModule : Module, ILog4NetModule
    {
        private readonly ILog4NetMiddleware _middleware;

        /// <summary>
        /// Default Constructor for the Log4NetModule.
        /// Creates the module with the default implementations for its dependencies.
        /// </summary>
        public Log4NetModule() :
            this(null, true)
        {
        }

        /// <summary>
        /// Creates the module with the default implementations for its dependencies.
        /// <param name="configFileName">The filename for the log4net config file</param>
        /// <param name="shouldWatchConfiguration">Enables watching for configuration changes in the file</param>
        /// </summary>
        public Log4NetModule(string configFileName = null, bool shouldWatchConfiguration = true) :
            this(new Log4NetMiddleware(configFileName, shouldWatchConfiguration))
        {
        }

        /// <summary>
        /// This constructor is mainly used for dependency injection in testing.
        /// Should not be used unless necessary to override the default implementations.
        /// </summary>
        /// <param name="middleware">Middleware to be used in the module</param>
        public Log4NetModule(ILog4NetMiddleware middleware)
        {
            _middleware = middleware;
        }

        /// <inheritdoc />
        public void MapTypeToLoggerName(Type type, string loggerName)
        {
            _middleware.MapTypeToLoggerName(type, loggerName);
        }

        /// <inheritdoc />
        public void MapNamespaceToLoggerName(string @namespace, string loggerName)
        {
            _middleware.MapNamespaceToLoggerName(@namespace, loggerName);
        }

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.PipelineBuilding += (sender, pipeline) =>
            {
                pipeline.Use(_middleware);
            };
            base.AttachToComponentRegistration(componentRegistry, registration);
        }

        protected override void Load(ContainerBuilder builder)
        {
            _middleware.Initialize();
            base.Load(builder);
        }
    }
}
