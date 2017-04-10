using Autofac.log4net.log4net;
using Autofac.log4net.Mapping;
using FluentAssertions;
using log4net;
using NSubstitute;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    [TestFixture]
    public class InjectionTests
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
        [TestCase("Logger1", TestName = "Should inject constructor parameter with Logger1")]
        [TestCase("Logger2", TestName = "Should inject constructor parameter with Logger2")]
        public void SHOULD_INJECT_CONSTRUCTOR_WITH_MAPPED_LOGGER(string loggerName)
        {
            //Arrange
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter, _loggerMapperAdapter);
            var fakeLogger = Substitute.For<ILog>();
            fakeLogger.Logger.Name.Returns(loggerName);
            _log4NetAdapter.GetLogger(loggerName).Returns(fakeLogger);
            builder.RegisterModule(loggingModule);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            _loggerMapperAdapter.GetLoggerName(typeof(InjectableClass)).Returns(loggerName);
            var container = builder.Build();

            //Act
            var resolved = container.Resolve<InjectableClass>();

            //Assert
            resolved.InternalLogger.Logger.Name.Should().Be(loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject property with Logger1")]
        [TestCase("Logger2", TestName = "Should inject property with Logger2")]
        public void SHOULD_INJECT_PROPERTY_WITH_MAPPED_LOGGER(string loggerName)
        {
            //Arrange
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter, _loggerMapperAdapter);
            var fakeLogger = Substitute.For<ILog>();
            fakeLogger.Logger.Name.Returns(loggerName);
            _log4NetAdapter.GetLogger(loggerName).Returns(fakeLogger);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            _loggerMapperAdapter.GetLoggerName(typeof(InjectableClass)).Returns(loggerName);
            var container = builder.Build();

            //Act
            var resolved = container.Resolve<InjectableClass>();

            //Assert
            resolved.PublicLogger.Logger.Name.Should().Be(loggerName);
        }
    }
}
