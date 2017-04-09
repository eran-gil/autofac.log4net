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
            var builder = new ContainerBuilder();
            builder.RegisterModule<Log4NetModule>();
            builder.RegisterType<InjectableClass>();
            var container = builder.Build();
            var instance = container.Resolve<InjectableClass>();
            instance.InternalLogger.Logger.Name.Should().Be(typeof(InjectableClass).ToString());
        }
    }
}
