namespace DiFrame;

public class ServiceDescriptor
{
    public Type ServiceType { get; init; } = default!;
    public Type? ImplementationType { get; set; }
    
    public object? Implementation { get; set; }
    public Func<ServiceProvider, object>? ImplementationFactory { get; set; }
    public ServiceLifetime Lifetime { get; set; }
}