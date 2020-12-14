using System;
using System.Collections.Generic;
using Autofac.log4net.Caching;
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
            var typeMapper = new CachedDictionaryLoggerMapper();
            var type = typeof(InjectableClass);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be(type.ToString());
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map InjectableClass to Logger1")]
        [TestCase("Logger2", TestName = "Should map InjectableClass to Logger2")]
        public void SHOULD_RETURN_LOGGER_NAME_FROM_TYPE_MAPPING(string expectedLoggerName)
        {
            //Arrange
            var typeMapper = new CachedDictionaryLoggerMapper();
            var type = typeof(InjectableClass);
            typeMapper.MapTypeToLoggerName(type, expectedLoggerName);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be(expectedLoggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map the namespace of InjectableClass to Logger1")]
        [TestCase("Logger2", TestName = "Should map the namespace of InjectableClass to Logger2")]
        public void SHOULD_RETURN_LOGGER_NAME_FROM_NAMESPACE_MAPPING(string expectedLoggerName)
        {
            //Arrange
            var typeMapper = new CachedDictionaryLoggerMapper();
            var type = typeof(InjectableClass);
            var @namespace = type.Namespace;
            typeMapper.MapNamespaceToLoggerName(@namespace, expectedLoggerName);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be(expectedLoggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map InjectableClass to Logger1 from constructor")]
        [TestCase("Logger2", TestName = "Should map InjectableClass to Logger2 from constructor")]
        public void SHOULD_RETURN_LOGGER_NAME_FROM_INITIAL_TYPE_MAPPING(string expectedLoggerName)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>
            {
                {typeof(InjectableClass), expectedLoggerName }
            };
            var emptyDictionary = new Dictionary<string, string>();
            var dummyCache = Substitute.For<IKeyValueCache<Type, string>>();
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, dummyCache);
            var type = typeof(InjectableClass);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be(expectedLoggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map the namespace of InjectableClass to Logger1 from constructor")]
        [TestCase("Logger2", TestName = "Should map the namespace of InjectableClass to Logger2 from constructor")]
        public void SHOULD_RETURN_LOGGER_NAME_FROM_INITIAL_NAMESPACE_MAPPING(string expectedLoggerName)
        {
            //Arrange
            var type = typeof(InjectableClass);
            var mappingDictionary = new Dictionary<string, string>
            {
                {type.Namespace, expectedLoggerName }
            };
            var emptyDictionary = new Dictionary<Type, string>();
            var dummyCache = Substitute.For<IKeyValueCache<Type, string>>();
            var typeMapper = new CachedDictionaryLoggerMapper(emptyDictionary, mappingDictionary, dummyCache);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be(expectedLoggerName);
        }

        [Test]
        public void SHOULD_USE_MOST_SPECIFIC_NAMESPACE_MAPPING()
        {
            //Arrange
            var generalNameSpace = "Autofac.log4net";
            var type = typeof(InjectableClass);
            var specificNameSpace = type.Namespace;
            var mappingDictionary = new Dictionary<string, string>
            {
                {generalNameSpace, "GeneralLogger" },
                {specificNameSpace, "SpecificLogger" },
            };
            var emptyDictionary = new Dictionary<Type, string>();
            var dummyCache = Substitute.For<IKeyValueCache<Type, string>>();
            var typeMapper = new CachedDictionaryLoggerMapper(emptyDictionary, mappingDictionary, dummyCache);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be("SpecificLogger");
        }

        [Test]
        public void SHOULD_USE_WIDER_NAMESPACE_MAPPING_IF_EXISTS()
        {
            //Arrange
            var generalNameSpace = "Autofac.log4net";
            var type = typeof(InjectableClass);
            var mappingDictionary = new Dictionary<string, string>
            {
                {generalNameSpace, "GeneralLogger" },
            };
            var emptyDictionary = new Dictionary<Type, string>();
            var dummyCache = Substitute.For<IKeyValueCache<Type, string>>();
            var typeMapper = new CachedDictionaryLoggerMapper(emptyDictionary, mappingDictionary, dummyCache);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be("GeneralLogger");
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map InjectableClass to Logger1 using the mapper")]
        [TestCase("Logger2", TestName = "Should map InjectableClass to Logger2 using the mapper")]
        public void SHOULD_USE_MAPPER_TO_MAP_TYPE_TO_LOGGER(string loggerName)
        {
            //Arrange
            var log4NetAdapter = Substitute.For<ILog4NetAdapter>();
            var typeLoggerMapperAdapter = Substitute.For<ILoggerMapper>();
            var module = new Log4NetMiddleware(log4NetAdapter, typeLoggerMapperAdapter);
            var type = typeof(InjectableClass);

            //Act
            module.MapTypeToLoggerName(type, loggerName);

            //Assert
            typeLoggerMapperAdapter.Received().MapTypeToLoggerName(type, loggerName);
        }

        [Test]
        [TestCase("Logger1", TestName = "Should map the namespace of InjectableClass to Logger1 using the mapper")]
        [TestCase("Logger2", TestName = "Should map the namespace of InjectableClass to Logger2 using the mapper")]
        public void SHOULD_USE_MAPPER_TO_MAP_NAMESPACE_TO_LOGGER(string loggerName)
        {
            //Arrange
            var log4NetAdapter = Substitute.For<ILog4NetAdapter>();
            var typeLoggerMapperAdapter = Substitute.For<ILoggerMapper>();
            var module = new Log4NetMiddleware(log4NetAdapter, typeLoggerMapperAdapter);
            var @namespace = typeof(InjectableClass).Namespace;

            //Act
            module.MapNamespaceToLoggerName(@namespace, loggerName);

            //Assert
            typeLoggerMapperAdapter.Received().MapNamespaceToLoggerName(@namespace, loggerName);
        }
    }
}
