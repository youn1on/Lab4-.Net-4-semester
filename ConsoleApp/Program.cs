using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace ConsoleApp;

public static class Program
{
    private const string Filename = "dictionary.txt";
    public static void Main()
    {
        ITranslator translator = new ProxyTranslator(Filename);
        UserInterface.MainLoop(translator);
    }
}