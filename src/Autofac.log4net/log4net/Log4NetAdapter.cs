using System.IO;
using log4net;
using log4net.Config;

namespace Autofac.log4net.log4net
{
    public class Log4NetAdapter : ILog4NetAdapter
    {
        public ILog GetLogger(string loggerName)
        {
            var logger = LogManager.GetLogger(loggerName);
            return logger;
        }

        public void ConfigureAndWatch(string fileName, bool shouldWatch)
        {
            if (fileName == null)
            {
                return;
            }

            var configFileInfo = new FileInfo(fileName);
            if (shouldWatch)
            {
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }
            else
            {
                XmlConfigurator.Configure(configFileInfo);
            }
        }
    }
}