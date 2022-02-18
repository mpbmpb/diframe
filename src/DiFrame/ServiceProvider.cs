namespace DiFrame;

public class ServiceProvider : IServiceProvider
{
    private Dictionary<Type, Lazy<object?>> _singletonTypes = new ();
    private Dictionary<Type, Func<object?>> _transientTypes = new();
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
                    var implementationTYpe = serviceDescriptor.ImplementationType;
                    var args = GetConstructorArguments(implementationTYpe);
                    _singletonTypes[serviceDescriptor.ServiceType] =  new(Activator.CreateInstance(
                        serviceDescriptor.ImplementationType, 
                        GetConstructorArguments(serviceDescriptor.ImplementationType)));
                    continue;
                
                case ServiceLifetime.Transient:
                    _transientTypes[serviceDescriptor.ServiceType] = () => Activator.CreateInstance(
                        serviceDescriptor.ImplementationType, 
                        GetConstructorArguments(serviceDescriptor.ImplementationType));
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