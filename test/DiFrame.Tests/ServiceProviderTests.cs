using System.IO;

namespace DiFrame.Tests;

public class ServiceProviderTests
{
    [Fact]
    public void GetService_ShouldGet_RegisteredSingleton()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConsoleWriter, ConsoleWriter>();

        var sut = serviceCollection.BuildServiceProvider();
        var result = sut.GetService<IConsoleWriter>();

        result.Should().BeOfType<ConsoleWriter>();
    }
    
    [Fact]
    public void GetService_ShouldGet_RegisteredTransient()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IConsoleWriter, ConsoleWriter>();

        var sut = serviceCollection.BuildServiceProvider();
        var result = sut.GetService<IConsoleWriter>();

        result.Should().BeOfType<ConsoleWriter>();
    }

    [Fact]
    public void RegisterServices_ShouldInject_RequiredDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConsoleWriter, ConsoleWriter>();
        serviceCollection.AddSingleton<IIdGenerator, IdGeneratorService>();
        ServiceProvider? sut = null;
        Exception? expectedException = null;

        try
        {
            sut = serviceCollection.BuildServiceProvider();
        }
        catch (Exception e)
        {
            expectedException = e;
        }
        var service = sut.GetService<IIdGenerator>();
        
        expectedException.Should().BeNull();
        service.Should().BeOfType<IdGeneratorService>();
    }

    [Fact]
    public void RegisterServices_ShouldRegisterSingleton_AsSingleton()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConsoleWriter, ConsoleWriter>();
        serviceCollection.AddSingleton<IIdGenerator, IdGeneratorService>();

        var sut = serviceCollection.BuildServiceProvider();
        var service = sut.GetService<IIdGenerator>();
        var service2 = sut.GetService<IIdGenerator>();
        var result1 = service.Id;
        var result2 = service2.Id;

        result1.Should().Be(result2);
    }

    [Fact]
    public void RegisterServices_ShouldRegisterTransient_AsTransient()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConsoleWriter, ConsoleWriter>();
        serviceCollection.AddTransient<IIdGenerator, IdGeneratorService>();

        var sut = serviceCollection.BuildServiceProvider();
        var service = sut.GetService<IIdGenerator>();
        var service2 = sut.GetService<IIdGenerator>();
        var result1 = service.Id;
        var result2 = service2.Id;

        result1.Should().NotBe(result2);
    }

    [Fact]
    public void RegisterServices_ShouldWork_WhenUsing_AddSingleton_WithNewObjectAsArgument()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(new ConsoleWriter());

        var sut = serviceCollection.BuildServiceProvider();
        var result = sut.GetService<ConsoleWriter>();

        result.Should().BeOfType<ConsoleWriter>();
    }
    
    [Fact]
    public void RegisterServices_ShouldWork_WhenUsing_AddSingleton_WithEmbeddedNewObjectsAsArgument()
    {
        using var output = new StringWriter();
        Console.SetOut(output);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(new IdGeneratorService(new ConsoleWriter()));

        var sut = serviceCollection.BuildServiceProvider();
        var service = sut.GetService<IdGeneratorService>();
        var service2 = sut.GetService<IdGeneratorService>();
        var result1 = service!.Id;
        var result2 = service2!.Id;
        service.Print();

        service.Should().BeOfType<IdGeneratorService>();
        result1.Should().Be(result2);
        output.ToString().Should().Contain(result1.ToString());
    }
    
     [Fact]
    public void RegisterServices_ShouldWork_WhenUsing_FactoryPattern_ForSingleton()
    {
        using var output = new StringWriter();
        Console.SetOut(output);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConsoleWriter, ConsoleWriter>();
        serviceCollection.AddSingleton(provider => 
            new IdGeneratorService(provider.GetRequiredService<IConsoleWriter>()));

        var sut = serviceCollection.BuildServiceProvider();
        var service = sut.GetService<IdGeneratorService>();
        var service2 = sut.GetService<IdGeneratorService>();
        var result1 = service!.Id;
        var result2 = service2!.Id;
        service.Print();

        service.Should().BeOfType<IdGeneratorService>();
        result1.Should().Be(result2);
        output.ToString().Should().Contain(result1.ToString());
    }
    
     [Fact]
    public void RegisterServices_ShouldWork_WhenUsing_FactoryPattern_ForTransient()
    {
        using var output = new StringWriter();
        Console.SetOut(output);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConsoleWriter, ConsoleWriter>();
        serviceCollection.AddTransient(provider => 
            new IdGeneratorService(provider.GetRequiredService<IConsoleWriter>()));

        var sut = serviceCollection.BuildServiceProvider();
        var service = sut.GetService<IdGeneratorService>();
        var service2 = sut.GetService<IdGeneratorService>();
        var result1 = service!.Id;
        var result2 = service2!.Id;
        service.Print();

        service.Should().BeOfType<IdGeneratorService>();
        result1.Should().NotBe(result2);
        output.ToString().Should().Contain(result1.ToString());
    }
    
    [Fact]
    public void GetService_ShouldReturnNull_ForUnregisteredType()
    {
        var serviceCollection = new ServiceCollection();

        var sut = serviceCollection.BuildServiceProvider();
        var result = sut.GetService<IConsoleWriter>();

        result.Should().BeNull();
    }

    [Fact]
    public void GetRequiredService_ShouldThrow_ForUnregisteredType()
    {
        var serviceCollection = new ServiceCollection();
        var sut = serviceCollection.BuildServiceProvider();
        Exception? expectedException = null;

        try
        {
            sut.GetRequiredService<IConsoleWriter>();
        }
        catch (Exception e)
        {
            expectedException = e;
        }

        expectedException.Should().BeOfType<InvalidOperationException>();
        expectedException!.Message.Should().Match("Service not found");
    }
    
}