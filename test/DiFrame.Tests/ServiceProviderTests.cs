namespace DiFrame.Tests;

public class ServiceProviderTests
{
    
    [Fact]
    public void RegisterServices_ShouldReturnServiceProvider_WithCorrectSingleton()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IIdGenerator, IdGeneratorService>();

        var sut = serviceCollection.BuildServiceProvider();
        var result = sut.GetService<IIdGenerator>();

        result.Should().BeOfType<IdGeneratorService>();
    }
    
    [Fact]
    public void RegisterServices_ShouldReturnServiceProvider_WithCorrectTransient()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IIdGenerator, IdGeneratorService>();

        var sut = serviceCollection.BuildServiceProvider();
        var result = sut.GetService<IIdGenerator>();

        result.Should().BeOfType<IdGeneratorService>();
    }

    [Fact]
    public void RegisterServices_ShouldRegisterSingleton_AsSingleton()
    {
        var serviceCollection = new ServiceCollection();
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
        serviceCollection.AddTransient<IIdGenerator, IdGeneratorService>();

        var sut = serviceCollection.BuildServiceProvider();
        var service = sut.GetService<IIdGenerator>();
        var service2 = sut.GetService<IIdGenerator>();
        var result1 = service.Id;
        var result2 = service2.Id;

        result1.Should().NotBe(result2);
    }


}