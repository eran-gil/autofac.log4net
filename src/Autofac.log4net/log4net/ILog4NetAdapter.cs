using log4net;

namespace Autofac.log4net.log4net
{
    /// <summary>
    /// This interface allows using the needed functions of the log4net framework to inject ILog object correctly
    /// </summary>
    public interface ILog4NetAdapter
    {
        /// <summary>
        /// Gets the logger according to the requested logger name.
        /// </summary>
        /// <param name="loggerName">Logger name to be retrieved from the log4net logger repository</param>
        /// <returns>Logger retrieved from log4net</returns>
        ILog GetLogger(string loggerName);

        /// <summary>
        /// In order to use a custom log4net configuration file, we need to configure log4net to use it.
        /// This method allows doing so, and configuring log4net to watch for changes in the configuration file and update during runtime.
        /// </summary>
        /// <param name="fileName">Relative/Full path to the configuration file</param>
        /// <param name="shouldWatch">Indicates whether log4net should watch the configuration file</param>
        void ConfigureAndWatch(string fileName, bool shouldWatch);
    }
}
