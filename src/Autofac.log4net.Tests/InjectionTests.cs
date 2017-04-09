using FluentAssertions;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    [TestFixture]
    public class InjectionTests
    {
        [Test]
        public void SHOULD_INJECT_LOGGER_WITH_CLASS_NAME()
        {
            var injectableType = typeof(InjectableClass);
            var builder = new ContainerBuilder();
            builder.RegisterModule<Log4NetModule>();
            builder.RegisterType<InjectableClass>();
            var container = builder.Build();
            var instance = container.Resolve<InjectableClass>();
            instance.InternalLogger.Logger.Name.Should().Be(injectableType.ToString());
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject constructor parameter with Logger1")]
        [TestCase("Logger2", TestName = "Should inject constructor parameter with Logger2")]
        public void SHOULD_INJECT_CONSTRUCTOR_WITH_MAPPED_LOGGER(string loggerName)
        {
            var injectableType = typeof(InjectableClass);
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule();
            loggingModule.MapTypeToLoggerName(injectableType, loggerName);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();

            var container = builder.Build();
            var instance = container.Resolve<InjectableClass>();
            instance.InternalLogger.Logger.Name.Should().Be(loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject constructor parameter without Logger1")]
        [TestCase("Logger2", TestName = "Should inject constructor parameter without Logger2")]
        public void SHOULD_INJECT_CONSTRUCTOR_WITHOUT_MAPPED_LOGGER(string loggerName)
        {
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule();
            loggingModule.MapTypeToLoggerName(typeof(InjectionTests), loggerName);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();

            var container = builder.Build();
            var instance = container.Resolve<InjectableClass>();
            instance.InternalLogger.Logger.Name.Should().NotBe(loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject property parameter with Logger1")]
        [TestCase("Logger2", TestName = "Should inject property parameter with Logger2")]
        public void SHOULD_INJECT_PROPERTY_WITH_MAPPED_LOGGER(string loggerName)
        {
            var injectableType = typeof(InjectableClass);
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule();
            loggingModule.MapTypeToLoggerName(injectableType, loggerName);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();

            var container = builder.Build();
            var instance = container.Resolve<InjectableClass>();
            instance.PublicLogger.Logger.Name.Should().Be(loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should inject property parameter without Logger1")]
        [TestCase("Logger2", TestName = "Should inject property parameter without Logger2")]
        public void SHOULD_INJECT_PROPERTY_WITHOUT_MAPPED_LOGGER(string loggerName)
        {
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule();
            loggingModule.MapTypeToLoggerName(typeof(InjectionTests), loggerName);
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();

            var container = builder.Build();
            var instance = container.Resolve<InjectableClass>();
            instance.PublicLogger.Logger.Name.Should().NotBe(loggerName);
        }
    }
}
