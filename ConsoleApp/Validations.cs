using System.Text.RegularExpressions;

namespace ConsoleApp;

public class Validations
{
    public static bool EnglishWordValidation(string word)
    {
        return Regex.IsMatch(word, @"^[A-Za-z\- ']");
    }
    public static bool UkrainianWordValidation(string word)
    {
        return word == "" || Regex.IsMatch(word, @"^[А-Яа-я\- 'ҐґЄєЇїIi]");
    }
}