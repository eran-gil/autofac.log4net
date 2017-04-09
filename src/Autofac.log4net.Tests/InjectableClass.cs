﻿using log4net;

namespace Autofac.log4net.Tests
{
    public class InjectableClass
    {
        internal ILog InternalLogger { get; private set; }

        public InjectableClass(ILog logger)
        {
            InternalLogger = logger;
        }
    }
}
