using System.Text.RegularExpressions;
using Infrastructure.Interfaces;

namespace ConsoleApp;

public static class UserInterface
{
    public static void MainLoop(ITranslator translator)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Type 'add' to add new word, 'get {englishWord}' to get translation for given word, or 'quit' to exit:");
            Console.Write(">>> ");
            Console.ResetColor();
            string input = Console.ReadLine()!;
            if (input == "quit")
                return;
            if (input == "add")
                AddNewWord(translator);
            else if (Regex.IsMatch(input, @"^get \w+$"))
                OutputTranslation(input, translator);
            else if (Regex.IsMatch(input, @"^add \w+$"))
                AddNewWord(translator, input);
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unresolved command!");
            }
        }
    }

    private static void OutputTranslation(string command, ITranslator translator)
    {
        string engWord = command.Split(' ')[1];
        var translation = translator.Translate(engWord);
        if (translation is null || !translation.Any())
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("No such word found!");
            return;
        }

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("English word: "+engWord);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Possible translations:");
        Console.WriteLine(string.Join(", ", translation));
        Console.WriteLine();
    }

    private static void AddNewWord(ITranslator translator, string? input = default)
    {
        string? englishWord;
        englishWord = input is not null
            ? input.Split(' ')[1]
            : GetInput("english word", Validations.EnglishWordValidation);
        
        if (englishWord is null) return;

        var translations = GetSerialInput("translation", Validations.UkrainianWordValidation, 1, 20);
        if (translations is null) return;

        if (translations.Count == 1)
        {
            if (!translator.Add(englishWord, translations[0]))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Such word already exists!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"New translation for word \"{englishWord}\" added!");
            }
        }
        else
        {
            translator.AddMultiple(englishWord, translations);
            Console.WriteLine($"Translations for word \"{englishWord}\" successfully added or updated!");
        }
    }

    private static string? GetInput(string fieldName, Func<string, bool> validationFunction)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Enter the {fieldName} or 'q' to exit: ");
        Console.ResetColor();
        
        var field = Console.ReadLine()!;
        if (field == "q")
            return null;
        
        while (!validationFunction(field))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Incorrect {fieldName} structure! Try again:");
            Console.ResetColor();
            field = Console.ReadLine()!;
            if (field == "q")
                return null;
        }

        return field;
    }
    
    private static List<string>? GetSerialInput(string fieldName, Func<string, bool> validationFunction, int minAmount, int maxAmount)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Enter from {minAmount} to {maxAmount} of {fieldName}s or 'q' to exit: ");
        Console.ResetColor();

        List<string> fieldParts = new List<string>();
        for (int i = 0; i < maxAmount; i++)
        {
            string? fieldPart = GetOptionalInput(fieldName, validationFunction);
            if (fieldPart is null)
                return null;
            if (fieldPart == "")
            {
                if (i < minAmount)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"You should input at least {minAmount} {fieldName}s!");
                    Console.ResetColor();
                    i--;
                    continue;
                }

                return fieldParts;
            }

            fieldParts.Add(fieldPart);
        }

        return fieldParts;
    }
    
    private static string? GetOptionalInput(string fieldName, Func<string, bool> validationFunction)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Enter the {fieldName} or 'q' to exit (leave empty to skip): ");
        Console.ResetColor();
        
        var field = Console.ReadLine()!;
        if (field == "q")
            return null;
        
        while (!validationFunction(field))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Incorrect {fieldName} structure! Try again:");
            Console.ResetColor();
            field = Console.ReadLine()!;
            if (field == "q")
                return null;
        }

        return field;
    }
}