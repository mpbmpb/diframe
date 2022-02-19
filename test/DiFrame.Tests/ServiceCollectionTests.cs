namespace DiFrame.Tests;

public class ServiceCollectionTests
{
    [Fact]
    public void AddSingleton_ShouldReturnServiceCollection_WithCorrectLifetimeRegistered()
    {
        var sut = new ServiceCollection();

        var result = sut.AddSingleton<IIdGenerator, IdGeneratorService>();

        result.FirstOrDefault()!.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddSingleton_ShouldReturnServiceCollection_WithCorrectTypesRegistered()
    {
        var sut = new ServiceCollection();
        var expectedDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(IIdGenerator),
            ImplementationType = typeof(IdGeneratorService),
            Lifetime = ServiceLifetime.Singleton
        };

        var result = sut.AddSingleton<IIdGenerator, IdGeneratorService>();

        result.FirstOrDefault().Should().BeEquivalentTo(expectedDescriptor);
    }

    [Fact]
    public void AddSingletonWithSingleType_ShouldReturnServiceCollection_WithCorrectTypesRegistered()
    {
        var sut = new ServiceCollection();
        var expectedDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(IdGeneratorService),
            ImplementationType = typeof(IdGeneratorService),
            Lifetime = ServiceLifetime.Singleton
        };

        var result = sut.AddSingleton<IdGeneratorService>();

        result.FirstOrDefault().Should().BeEquivalentTo(expectedDescriptor);
    }

    [Fact]
    public void AddTransient_ShouldReturnServiceCollection_WithCorrectLifetimeRegistered()
    {
        var sut = new ServiceCollection();
        var result = sut.AddTransient<IIdGenerator, IdGeneratorService>();

        result.FirstOrDefault()!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTransient_ShouldReturnServiceCollection_WithCorrectTypesRegistered()
    {
        var sut = new ServiceCollection();
        var expectedDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(IIdGenerator),
            ImplementationType = typeof(IdGeneratorService),
            Lifetime = ServiceLifetime.Transient
        };

        var result = sut.AddTransient<IIdGenerator, IdGeneratorService>();

        result.FirstOrDefault().Should().BeEquivalentTo(expectedDescriptor);
    }

    [Fact]
    public void AddTransientWithSingleType_ShouldReturnServiceCollection_WithCorrectTypesRegistered()
    {
        var sut = new ServiceCollection();
        var expectedDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(IdGeneratorService),
            ImplementationType = typeof(IdGeneratorService),
            Lifetime = ServiceLifetime.Transient
        };

        var result = sut.AddTransient<IdGeneratorService>();

        result.FirstOrDefault().Should().BeEquivalentTo(expectedDescriptor);
    }


    [Fact]
    public void BuildServiceProvider_ShouldReturn_ServiceProvider()
    {
        var sut = new ServiceCollection();

        var result = sut.BuildServiceProvider();

        result.Should().BeOfType<ServiceProvider>();
    }

    [Fact]
    public void AddSingleton_WithNewObjectAsArgument_ShouldReturnServiceCollection_WithCorrectTypeRegistered()
    {
        var sut = new ServiceCollection();
        var expectedDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(ConsoleWriter),
            ImplementationType = typeof(ConsoleWriter),
            Implementation = new ConsoleWriter(),
            Lifetime = ServiceLifetime.Singleton
        };

        var result = sut.AddSingleton(new ConsoleWriter());

        result.FirstOrDefault().Should().BeEquivalentTo(expectedDescriptor);
    }
    
    [Fact]
    public void AddSingleton_WithRecursiveNewObjectAsArgument_ShouldReturnServiceCollection_WithCorrectTypeRegistered()
    {
        var sut = new ServiceCollection();
        var service = new IdGeneratorService(new ConsoleWriter());
        var expectedDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(IdGeneratorService),
            ImplementationType = typeof(IdGeneratorService),
            Implementation = service,
            Lifetime = ServiceLifetime.Singleton
        };

        var result = sut.AddSingleton(service);

        result.FirstOrDefault().Should().BeEquivalentTo(expectedDescriptor);
    }
    
    [Fact]
    public void AddSingleton_WithFactoryPattern_ShouldReturnServiceCollection_WithCorrectFuncRegistered()
    {
        var sut = new ServiceCollection();

        sut.AddSingleton<IConsoleWriter, ConsoleWriter>();
        var result = sut.AddSingleton(provider => new IdGeneratorService(provider.GetRequiredService<IConsoleWriter>()));

        result[1].ImplementationFactory.Should().BeOfType<Func<ServiceProvider, IdGeneratorService>>();
        result[1].Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
    
   [Fact]
    public void AddTransient_WithFactoryPattern_ShouldReturnServiceCollection_WithCorrectFuncRegistered()
    {
        var sut = new ServiceCollection();

        sut.AddSingleton<IConsoleWriter, ConsoleWriter>();
        var result = sut.AddTransient(provider => new IdGeneratorService(provider.GetRequiredService<IConsoleWriter>()));

        result[1].ImplementationFactory.Should().BeOfType<Func<ServiceProvider, IdGeneratorService>>();
        result[1].Lifetime.Should().Be(ServiceLifetime.Transient);
    }
    
    [Fact]
    public void AddService_ShouldReturnServiceCollection_WithCorrectTypesRegistered()
    {
        var sut = new ServiceCollection();
        var descriptor = new ServiceDescriptor
        {
            ServiceType = typeof(IIdGenerator),
            ImplementationType = typeof(IdGeneratorService),
            Lifetime = ServiceLifetime.Singleton
        };

        var result = sut.AddService(descriptor);

        result.FirstOrDefault().Should().BeEquivalentTo(descriptor);
    }
}