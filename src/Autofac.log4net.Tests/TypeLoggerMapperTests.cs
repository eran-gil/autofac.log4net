using System;
using System.Collections.Generic;
using Autofac.log4net.log4net;
using Autofac.log4net.Mapping;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    [TestFixture]
    public class TypeLoggerMapperTests
    {
        [Test]
        public void SHOULD_RETURN_TYPE_STRING_AS_LOGGER_NAME()
        {
            //Arrange
            var typeMapper = new DictionaryLoggerMapper();
            var type = typeof(InjectableClass);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be(type.ToString());
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map InjectableClass to Logger1")]
        [TestCase("Logger2", TestName = "Should map InjectableClass to Logger2")]
        public void SHOULD_RETURN_LOGGER_NAME_FROM_MAPPING(string expectedLoggerName)
        {
            //Arrange
            var typeMapper = new DictionaryLoggerMapper();
            var type = typeof(InjectableClass);
            typeMapper.MapTypeToLoggerName(type, expectedLoggerName);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be(expectedLoggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map InjectableClass to Logger1 from constructor")]
        [TestCase("Logger2", TestName = "Should map InjectableClass to Logger2 from constructor")]
        public void SHOULD_RETURN_LOGGER_NAME_FROM_INITIAL_MAPPING(string expectedLoggerName)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>
            {
                {typeof(InjectableClass), expectedLoggerName }
            };
            var emptyDictionary = new Dictionary<string, string>();
            var typeMapper = new DictionaryLoggerMapper(mappingDictionary, emptyDictionary);
            var type = typeof(InjectableClass);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be(expectedLoggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map InjectableClass to Logger1 using the mapper")]
        [TestCase("Logger2", TestName = "Should map InjectableClass to Logger2 using the mapper")]
        public void SHOULD_USE_MAPPER_TO_MAP_TYPE_TO_LOGGER(string loggerName)
        {
            //Arrange
            var log4NetAdapter = Substitute.For<ILog4NetAdapter>();
            var typeLoggerMapperAdapter = Substitute.For<ILoggerMapper>();
            var module = new Log4NetModule(log4NetAdapter, typeLoggerMapperAdapter);
            var type = typeof(InjectableClass);

            //Act
            module.MapTypeToLoggerName(type, loggerName);

            //Assert
            typeLoggerMapperAdapter.Received().MapTypeToLoggerName(type, loggerName);
        }
    }
}
