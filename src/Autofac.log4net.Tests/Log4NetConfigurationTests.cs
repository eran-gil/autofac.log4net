using Autofac.log4net.log4net;
using Autofac.log4net.Mapping;
using NSubstitute;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    [TestFixture]
    public class Log4NetConfigurationTests
    {
        private ILog4NetAdapter _log4NetAdapter;
        private ILoggerMapper _loggerMapperAdapter;

        [SetUp]
        public void SetupTests()
        {
            _log4NetAdapter = Substitute.For<ILog4NetAdapter>();
            _loggerMapperAdapter = Substitute.For<ILoggerMapper>();
        }

        [Test]
        public void SHOULD_LOAD_NULL_CONFIGURATION_FILE()
        {
            //Arrange
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter, _loggerMapperAdapter);
            builder.RegisterModule(loggingModule);

            //Act
            builder.Build();

            //Assert
            _log4NetAdapter.Received().ConfigureAndWatch(null, true);

        }

        [Test]
        [TestCase("Logger1.config", true, TestName = "Should configure and watch Logger1.config")]
        [TestCase("Logger2.config", true, TestName = "Should configure and watch Logger2.config")]
        [TestCase("Logger1.config", false, TestName = "Should configure and not watch Logger1.config")]
        [TestCase("Logger2.config", false, TestName = "Should configure and not watch Logger2.config")]
        public void SHOULD_LOAD_CONFIGURATION_FILE(string configFileName, bool shouldWatch)
        {
            //Arrange
            var builder = new ContainerBuilder();
            var loggingModule =
                new Log4NetModule(_log4NetAdapter, _loggerMapperAdapter)
                {
                    ConfigFileName = configFileName,
                    ShouldWatchConfiguration = shouldWatch
                };
            builder.RegisterModule(loggingModule);

            //Act
            builder.Build();

            //Assert
            _log4NetAdapter.Received().ConfigureAndWatch(configFileName, shouldWatch);
        }
    }
}
