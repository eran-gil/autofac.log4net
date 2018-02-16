using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace Autofac.log4net.log4net
{
    internal class Log4NetAdapter : ILog4NetAdapter
    {
        public ILog GetLogger(string loggerName)
        {
            var logger = LogManager.GetLogger(Assembly.GetCallingAssembly(), loggerName);
            return logger;
        }

        public void ConfigureAndWatch(string fileName, bool shouldWatch)
        {
            if (fileName == null)
            {
                return;
            }

            var repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            var configFileInfo = new FileInfo(fileName);
            if (shouldWatch)
            {
                XmlConfigurator.ConfigureAndWatch(repository, configFileInfo);
            }
            else
            {
                XmlConfigurator.Configure(repository, configFileInfo);
            }
        }
    }
}