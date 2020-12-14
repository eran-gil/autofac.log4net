using Autofac.Core.Resolving.Pipeline;

namespace Autofac.log4net
{
    public interface ILog4NetMiddleware : IResolveMiddleware
    {
        void Initialize();
    }
}