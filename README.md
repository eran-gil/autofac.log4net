Autofac.log4net
=====================
## Description
Autofac.log4net is a library that allows easy integration of log4net with the Autofac IoC container.
It contains a Log4NetModule to support injection of ILog properties and constructor parameter to instances created through the Autofac container.

![VisualStudioBuild](https://erangil.visualstudio.com/_apis/public/build/definitions/03a1fe3b-e4f9-4387-a1e8-6f577710315e/4/badge) [![NuGet version](https://badge.fury.io/nu/autofac.log4net.svg)](https://badge.fury.io/nu/autofac.log4net)

## Requirements
- Supports .NET 4 and above.
- The package has 2 package dependencies:
    - log4net >= 2.0.3
    - Autofac >= 3.0.0
## Features
- If a class has a property or constructor parameter of type ILog, it will inject it with a logger.
    - The logger instance will be by default the logger with the class type name.
    - If the type/namespace of the class is mapped in the module, the logger instance will be the mapped logger.
- There are 2 type of mappings the Log4NetModule:
    1. Mapping by types.
    2. Mapping by namespaces - when injecting the logger, the most specific namespace mapping will be used.
- The module allows configuring the application to a custom logger configuration file and watching it.

## Examples

#### Class with an ILog Constructor Parameter
```cs
public class InjectableClass {
    private readonly ILog _logger;
    
    public InjectableClass(ILog logger){
        _logger = logger;
    }
}
```

#### Class with an ILog Property
```cs
public class InjectableClass {
    public ILog Logger { get; set; }
}
```

#### Simple Module Registration
```cs
var builder = new ContainerBuilder();
builder.RegisterModule<Log4NetModule>();
```

#### Custom Module Registration
```cs
var builder = new ContainerBuilder();
var loggingModule =
    new Log4NetModule()
    {
        ConfigFileName = "logger.config",
        ShouldWatchConfiguration = true
    };
builder.RegisterModule(loggingModule);
```

#### Mapping Types and Namespaces to Loggers
```cs
var builder = new ContainerBuilder();
var loggingModule = new Log4NetModule();
loggingModule.MapTypeToLoggerName(typeof(InjectableClass), "Logger1");
loggingModule.MapNamespaceToLoggerName("Autofac.log4net", "Logger2");
builder.RegisterModule(loggingModule);
```

