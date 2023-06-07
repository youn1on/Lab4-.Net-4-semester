using System.Text;
using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class Translator : ITranslator
{
    private string _filename;
    public Translator(string sourceFileName)
    {
        _filename = sourceFileName;
    }
    
    public List<string>? Translate(string englishWord)
    {
        var content = ReadFromFile(_filename);
        var wordPairIndex = content.IndexOfKey(englishWord);
        return wordPairIndex == -1 ? null : content[englishWord];
    }

    public bool Add(string englishWord, string ukrainianTranslation)
    {
        var content = ReadFromFile(_filename);
        var wordPairIndex = content.IndexOfKey(englishWord);
        if (wordPairIndex != -1)
        {
            if (content[englishWord].Contains(ukrainianTranslation))
                return false;
            content[englishWord].Add(ukrainianTranslation);
        }
        else
            content.Add(englishWord, new List<string>(new[]{ ukrainianTranslation }));

        File.WriteAllText(_filename, ToFileFormat(content));
        return true;
    }

    public void AddMultiple(string englishWord, IEnumerable<string> ukrainianTranslations)
    {
        var content = ReadFromFile(_filename);
        var wordPairIndex = content.IndexOfKey(englishWord);
        if (wordPairIndex != -1)
        {
            var currentTranslations = content[englishWord];
            foreach (var ukrainianTranslation in ukrainianTranslations)
            {
                if (currentTranslations.Contains(ukrainianTranslation))
                    return;
                currentTranslations.Add(ukrainianTranslation);
            }
        }
        else
            content.Add(englishWord, new List<string>(ukrainianTranslations));
            
        File.WriteAllText(_filename, ToFileFormat(content));
    }

    private static SortedList<string, List<string>> ReadFromFile(string filename)
    {
        SortedList<string, List<string>> contentMap = new();
        if (!File.Exists(filename))
            return contentMap;
        
        var content = File.ReadAllLines(filename);
        foreach (var line in content)
        {
            var splitted = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length != 2)
                continue;

            List<string> meanings = new List<string>(splitted[1].Split(',', StringSplitOptions.RemoveEmptyEntries));
            contentMap.Add(splitted[0], meanings);
        }

        return contentMap;
    }

    private static string ToFileFormat(SortedList<string, List<string>> content)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var value in content)
        {
            builder.Append(value.Key + ":");
            builder.AppendJoin(",", value.Value);

            builder.Append('\n');
        }

        return builder.ToString();
    }
}