using log4net;

namespace Autofac.log4net.log4net
{
    public interface ILog4NetAdapter
    {
        ILog GetLogger(string loggerName);

        void ConfigureAndWatch(string fileName, bool shouldWatch);
    }
}
