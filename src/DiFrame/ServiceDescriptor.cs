namespace DiFrame;

public class ServiceDescriptor
{
    public Type ServiceType { get; init; } = default!;
    public Type ImplementationType { get; set; }
    public ServiceLifetime Lifetime { get; set; }
}