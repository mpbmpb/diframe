using System;

namespace DiFrame.Tests.Helpers;

public class IdGeneratorService : IIdGenerator
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public interface IIdGenerator
{
    public Guid Id { get; set; }
}