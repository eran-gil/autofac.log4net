using System;
using System.Collections.Generic;
using Autofac.log4net.Caching;
using Autofac.log4net.Mapping;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Autofac.log4net.Tests
{
    [TestFixture]
    public class TypeMappingCacheTests
    {
        [Test]
        [TestCase(typeof(InjectableClass), TestName = "Should attempt finding InjectableClass in the cache")]
        public void SHOULD_ATTEMPT_USING_CACHE_FOR_EACH_INJECTION(Type type)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            typeMapper.GetLoggerName(type);

            //Assert
            cache.Received().ContainsKey(type);
        }

        [Test]
        [TestCase(typeof(InjectableClass), TestName = "Should get logger name for InjectableClass from the cache")]
        public void SHOULD_GET_LOGGER_NAME_FROM_CACHE_WHEN_EXISTS(Type type)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            cache.ContainsKey(type).Returns(true);
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            typeMapper.GetLoggerName(type);

            //Assert
            cache.Received().GetEntryValue(type);
        }

        [Test]
        [TestCase(typeof(InjectableClass), TestName = "Should not get logger name for InjectableClass from the cache")]
        public void SHOULD_NOT_GET_LOGGER_NAME_FROM_CACHE_WHEN_DOES_NOT_EXIST(Type type)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            cache.ContainsKey(type).Returns(false);
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            typeMapper.GetLoggerName(type);

            //Assert
            cache.DidNotReceive().GetEntryValue(type);
        }

        [Test]
        [TestCase(typeof(InjectableClass), TestName = "Should return logger name for InjectableClass from the cache")]
        public void SHOULD_RETURN_LOGGER_NAME_FROM_CACHE_WHEN_EXISTS(Type type)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            cache.ContainsKey(type).Returns(true);
            cache.GetEntryValue(type).Returns("CachedLogger");
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().Be("CachedLogger");
        }

        [Test]
        [TestCase(typeof(InjectableClass), TestName = "Should not return logger name for InjectableClass from the cache")]
        public void SHOULD_NOT_RETURN_LOGGER_NAME_FROM_CACHE_WHEN_DOES_NOT_EXIST(Type type)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            cache.ContainsKey(type).Returns(false);
            cache.GetEntryValue(type).Returns("CachedLogger");
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            loggerName.Should().NotBe("CachedLogger");
        }

        [Test]
        [TestCase(typeof(InjectableClass), TestName = "Should add logger name for InjectableClass to the cache ")]
        public void SHOULD_ADD_LOGGER_NAME_TO_CACHE_WHEN_DOES_NOT_EXIST(Type type)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            cache.ContainsKey(type).Returns(false);
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            cache.Received().AddEntry(type, loggerName);
        }

        [Test]
        [TestCase(typeof(InjectableClass), TestName = "Should not add logger name for InjectableClass to the cache ")]
        public void SHOULD_NOT_ADD_LOGGER_NAME_TO_CACHE_WHEN_EXISTS(Type type)
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            cache.ContainsKey(type).Returns(true);
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            var loggerName = typeMapper.GetLoggerName(type);

            //Assert
            cache.DidNotReceive().AddEntry(type, loggerName);
        }

        [Test]
        public void SHOULD_CLEAR_CACHE_WHEN_ADDING_TYPE_MAPPING()
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            typeMapper.MapTypeToLoggerName(typeof(InjectableClass), "Logger1");

            //Assert
            cache.Received().Clear();
        }

        [Test]
        public void SHOULD_CLEAR_CACHE_WHEN_ADDING_NAMESPACE_MAPPING()
        {
            //Arrange
            var mappingDictionary = new Dictionary<Type, string>();
            var emptyDictionary = new Dictionary<string, string>();
            var cache = Substitute.For<IKeyValueCache<Type, string>>();
            var typeMapper = new CachedDictionaryLoggerMapper(mappingDictionary, emptyDictionary, cache);

            //Act
            typeMapper.MapNamespaceToLoggerName("namespace", "Logger1");

            //Assert
            cache.Received().Clear();
        }

        [Test]
        public void SHOULD_INDICATE_EXISTENCE_WHEN_VALUE_IS_CACHED()
        {
            //Arrange
            var cache = new DictionaryKeyValueCache<Type, string>();
            var type = typeof(InjectableClass);
            cache.AddEntry(type, "string1");

            //Act
            var exists = cache.ContainsKey(type);

            //Assert
            exists.Should().BeTrue();
        }

        [Test]
        public void SHOULD_INDICATE_ABSENCE_WHEN_VALUE_IS_NOT_CACHED()
        {
            //Arrange
            var cache = new DictionaryKeyValueCache<Type, string>();
            var type = typeof(InjectableClass);

            //Act
            var exists = cache.ContainsKey(type);

            //Assert
            exists.Should().BeFalse();
        }

        [Test]
        public void SHOULD_RETURN_CACHED_VALUE_FOR_CACHED_ENTRY()
        {
            //Arrange
            var cache = new DictionaryKeyValueCache<Type, string>();
            var type = typeof(InjectableClass);
            cache.AddEntry(type, "CachedValue");

            //Act
            var value = cache.GetEntryValue(type);

            //Assert
            value.Should().Be("CachedValue");
        }
    }
}
