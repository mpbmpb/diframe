namespace DiFrame;

public class ServiceCollection : List<ServiceDescriptor>
{
    public ServiceCollection AddSingleton<TService, TImplementation>()
    {
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TImplementation),
            Lifetime = ServiceLifetime.Singleton
        };
        Add(serviceDescriptor);
        return this;
    }

    public ServiceCollection AddTransient<TService, TImplementation>()
    {
        var serviceDescriptor = new ServiceDescriptor
        {
            ServiceType = typeof(TService),
            ImplementationType = typeof(TImplementation),
            Lifetime = ServiceLifetime.Transient
        };
        Add(serviceDescriptor);
        return this;
    }

    public ServiceProvider BuildServiceProvider()
    {
        var serviceProvider = new ServiceProvider();

        return serviceProvider;
    }
}