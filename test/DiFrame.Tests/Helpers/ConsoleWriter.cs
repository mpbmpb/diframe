using System;

namespace DiFrame.Tests.Helpers;

public class ConsoleWriter : IConsoleWriter
{
    public void Write(string text)
    {
        Console.WriteLine(text);
    }
}