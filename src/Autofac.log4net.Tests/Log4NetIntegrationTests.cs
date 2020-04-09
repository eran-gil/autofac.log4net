using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using log4net;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    class Log4NetIntegrationTests
    {
        [Test]
        [TestCase("Logger1.config", false, TestName = "Should configure by Logger1.config")]
        [TestCase("Logger2.config", true, TestName = "Should configure by Logger2.config")]
        public void SHOULD_LOAD_CONFIGURATION_FILE(string configFileName, bool isInfoEnabled)
        {
            //Arrange
            var builder = new ContainerBuilder();
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), configFileName);
            fileName = fileName.Replace("file:\\", "");
            var loggingModule =
                new Log4NetModule
                {
                    ConfigFileName = fileName,

                    ShouldWatchConfiguration = false
                };
            builder.RegisterModule(loggingModule);
            builder.RegisterType<InjectableClass>();
            var container = builder.Build();

            //Act
            var resolved = container.Resolve<InjectableClass>();

            //Assertu
            resolved.InternalLogger.IsDebugEnabled.Should().BeFalse();
            resolved.InternalLogger.IsInfoEnabled.Should().Be(isInfoEnabled);
        }
    }
}
