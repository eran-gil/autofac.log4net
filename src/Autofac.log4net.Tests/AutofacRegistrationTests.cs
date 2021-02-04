using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    [TestFixture]
    public class AutofacRegistrationTests
    {
        [Test]
        public void SHOULD_BE_ABLE_TO_REGISTER_IMPLICITLY()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<Log4NetModule>();
        }

        [Test]
        public void SHOULD_BE_ABLE_TO_REGISTER_WITH_EXPLICIT_PARAMETERS()
        {
            var builder = new ContainerBuilder();
            var loggingModule = new Log4NetModule("logger.config", true);
            builder.RegisterModule(loggingModule);
        }
    }
}
