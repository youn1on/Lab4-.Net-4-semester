using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class ProxyTranslator : ITranslator
{
    private ITranslator _translator;
    private Dictionary<string, List<string>> _cachedWords;
    public ProxyTranslator(string sourceFileName)
    {
        _translator = new Translator(sourceFileName);
        _cachedWords = new Dictionary<string, List<string>>();
    }
    
    public List<string>? Translate(string englishWord)
    {
        if (_cachedWords.ContainsKey(englishWord))
            return _cachedWords[englishWord];

        var result = _translator.Translate(englishWord);
        if (result is not null) _cachedWords[englishWord] = result;
        return result;
    }

    public bool Add(string englishWord, string ukrainianTranslation)
    {
        var result = _translator.Add(englishWord, ukrainianTranslation);
        var translations = _translator.Translate(englishWord)!;
        _cachedWords[englishWord] = translations;
        return result;
    }

    public void AddMultiple(string englishWord, IEnumerable<string> ukrainianTranslations)
    {
        _translator.AddMultiple(englishWord, ukrainianTranslations);
        var translations = _translator.Translate(englishWord)!;
        _cachedWords[englishWord] = translations;
    }
}