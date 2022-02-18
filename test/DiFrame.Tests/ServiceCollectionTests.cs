using System.Linq;
using DiFrame.Tests.Helpers;
using FluentAssertions;
using Xunit;

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
    public void BuildServiceProvider_ShouldReturn_ServiceProvider()
    {
        var sut = new ServiceCollection();

        var result = sut.BuildServiceProvider();

        result.Should().BeOfType<ServiceProvider>();
    }
}