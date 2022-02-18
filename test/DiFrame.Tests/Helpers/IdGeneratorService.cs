namespace DiFrame.Tests.Helpers;

public class IdGeneratorService : IIdGenerator
{
    private readonly IConsoleWriter _consoleWriter;
    public Guid Id { get; set; } = Guid.NewGuid();

    public IdGeneratorService(IConsoleWriter consoleWriter)
    {
        _consoleWriter = consoleWriter;
    }

    public void Print()
    {
        _consoleWriter.Write(Id.ToString());
    }
}

public interface IIdGenerator
{
    public Guid Id { get; set; }
    public void Print();
}