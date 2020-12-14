
using Autofac.Core;
using Autofac.Core.Registration;

namespace Autofac.log4net
{
    public class Log4NetModule : Module
    {
        private readonly ILog4NetMiddleware _middleware;

        public Log4NetModule(string configFileName, bool shouldWatchConfiguration)
        {
            _middleware = new Log4NetMiddleware(configFileName, shouldWatchConfiguration);
        }

        public Log4NetModule(ILog4NetMiddleware middleware)
        {
            _middleware = middleware;
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
