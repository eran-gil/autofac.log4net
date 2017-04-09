using Autofac.log4net.log4net;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    [TestFixture]
    public class InjectionTests
    {
        private ILog4NetAdapter _log4NetAdapter;

        [SetUp]
        public void SetupTests()
        {
            _log4NetAdapter = Substitute.For<ILog4NetAdapter>();
        }

        [Test]
        public void SHOULD_INJECT_LOGGER_WITH_CLASS_NAME()
        {
            //Arrange
            var injectableType = typeof(InjectableClass);
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            var container = builder.Build();
            
            //Act
            container.Resolve<InjectableClass>();

            //Assert
            _log4NetAdapter.Received().GetLogger(injectableType.ToString());
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject constructor parameter with Logger1")]
        [TestCase("Logger2", TestName = "Should inject constructor parameter with Logger2")]
        public void SHOULD_INJECT_CONSTRUCTOR_WITH_MAPPED_LOGGER(string loggerName)
        {
            //Arrange
            var injectableType = typeof(InjectableClass);
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter);
            builder.RegisterModule(loggingModule);
            loggingModule.MapTypeToLoggerName(injectableType, loggerName);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            var container = builder.Build();

            //Act
            container.Resolve<InjectableClass>();

            //Assert
            _log4NetAdapter.Received().GetLogger(loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject constructor parameter without Logger1")]
        [TestCase("Logger2", TestName = "Should inject constructor parameter without Logger2")]
        public void SHOULD_INJECT_CONSTRUCTOR_WITHOUT_MAPPED_LOGGER(string loggerName)
        {
            //Arrange
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter);
            loggingModule.MapTypeToLoggerName(typeof(InjectionTests), loggerName);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            var container = builder.Build();

            //Act
            container.Resolve<InjectableClass>();

            //Assert
            _log4NetAdapter.DidNotReceive().GetLogger(loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject property with Logger1")]
        [TestCase("Logger2", TestName = "Should inject property with Logger2")]
        public void SHOULD_INJECT_PROPERTY_WITH_MAPPED_LOGGER(string loggerName)
        {
            //Arrange
            var injectableType = typeof(InjectableClass);
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter);
            loggingModule.MapTypeToLoggerName(injectableType, loggerName);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            var container = builder.Build();

            //Act
            container.Resolve<InjectableClass>();

            //Assert
            _log4NetAdapter.Received().GetLogger(loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject property without Logger1")]
        [TestCase("Logger2", TestName = "Should inject property without Logger2")]
        public void SHOULD_INJECT_PROPERTY_WITHOUT_MAPPED_LOGGER(string loggerName)
        {
            //Arrange
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter);
            loggingModule.MapTypeToLoggerName(typeof(InjectionTests), loggerName);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            var container = builder.Build();

            //Act
            container.Resolve<InjectableClass>();

            //Assert
            _log4NetAdapter.DidNotReceive().GetLogger(loggerName);
        }
    }
}
