namespace DYI_DI_Frame;

public class ConsoleWriter : IConsoleWriter
{
    public void Write(string text)
    {
        Console.WriteLine(text);
    }
}