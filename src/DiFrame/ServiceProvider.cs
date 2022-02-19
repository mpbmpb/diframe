namespace DiFrame;

public class ServiceProvider
{
    private readonly Dictionary<Type, Lazy<object?>> _singletonTypes = new ();
    private readonly Dictionary<Type, Func<object?>> _transientTypes = new();
    public T? GetService<T>()
    {
        return (T?) GetService(typeof(T));
    }
    public object? GetService(Type serviceType)
    {
        if (_singletonTypes.ContainsKey(serviceType))
            return _singletonTypes[serviceType].Value;

        return _transientTypes.GetValueOrDefault(serviceType)?.Invoke();
    }
    public T GetRequiredService<T>()
    {
        return (T) GetRequiredService(typeof(T));
    }
    public object GetRequiredService(Type serviceType)
    {
        var service = GetService(serviceType);

        if (service is null)
            throw new InvalidOperationException("Service not found");

        return service;
    }

    internal void RegisterServices(ServiceCollection services)
    {
        foreach (var serviceDescriptor in services)
        {
            switch (serviceDescriptor.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    if (serviceDescriptor.Implementation is not null)
                    {
                        _singletonTypes[serviceDescriptor.ServiceType] = new(serviceDescriptor.Implementation);
                        continue;
                    }

                    if (serviceDescriptor.ImplementationFactory is not null)
                    {
                        _singletonTypes[serviceDescriptor.ServiceType] =
                            new(serviceDescriptor.ImplementationFactory(this));
                        continue;
                    }
                    
                    _singletonTypes[serviceDescriptor.ServiceType] =  new(Activator.CreateInstance(
                        serviceDescriptor.ImplementationType!, 
                        GetConstructorArguments(serviceDescriptor.ImplementationType!)));
                    continue;
                
                case ServiceLifetime.Transient:
                    if (serviceDescriptor.ImplementationFactory is not null)
                    {
                        _transientTypes[serviceDescriptor.ServiceType] = () => serviceDescriptor.ImplementationFactory(this);
                        continue;
                    }
                    _transientTypes[serviceDescriptor.ServiceType] = () => Activator.CreateInstance(
                        serviceDescriptor.ImplementationType!, 
                        GetConstructorArguments(serviceDescriptor.ImplementationType!));
                    continue;
            }
        }
    }

    private object?[] GetConstructorArguments(Type implementationTYpe)
    {
        return implementationTYpe.GetConstructors().First().GetParameters()
            .Select(p => GetService(p.ParameterType)).ToArray();
    }
}