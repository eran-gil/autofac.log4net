using Autofac.log4net.log4net;
using Autofac.log4net.Mapping;
using NSubstitute;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    [TestFixture]
    public class InjectionTests
    {
        private ILog4NetAdapter _log4NetAdapter;
        private ITypeLoggerMapper _typeLoggerMapperAdapter;

        [SetUp]
        public void SetupTests()
        {
            _log4NetAdapter = Substitute.For<ILog4NetAdapter>();
            _typeLoggerMapperAdapter = Substitute.For<ITypeLoggerMapper>();
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject constructor parameter with Logger1")]
        [TestCase("Logger2", TestName = "Should inject constructor parameter with Logger2")]
        public void SHOULD_INJECT_CONSTRUCTOR_WITH_MAPPED_LOGGER(string loggerName)
        {
            //Arrange
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter, _typeLoggerMapperAdapter);
            builder.RegisterModule(loggingModule);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            _typeLoggerMapperAdapter.GetLoggerName(typeof(InjectableClass)).Returns(loggerName);
            var container = builder.Build();

            //Act
            container.Resolve<InjectableClass>();

            //Assert
            _log4NetAdapter.Received().GetLogger(loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject property with Logger1")]
        [TestCase("Logger2", TestName = "Should inject property with Logger2")]
        public void SHOULD_INJECT_PROPERTY_WITH_MAPPED_LOGGER(string loggerName)
        {
            //Arrange
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule(_log4NetAdapter, _typeLoggerMapperAdapter);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            _typeLoggerMapperAdapter.GetLoggerName(typeof(InjectableClass)).Returns(loggerName);
            var container = builder.Build();

            //Act
            container.Resolve<InjectableClass>();

            //Assert
            _log4NetAdapter.Received().GetLogger(loggerName);
        }
    }
}
