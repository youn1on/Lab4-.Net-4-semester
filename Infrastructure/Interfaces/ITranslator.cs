namespace Infrastructure.Interfaces;

public interface ITranslator
{
    public List<string>? Translate(string englishWord);
    public bool Add(string englishWord, string ukrainianTranslation);
    public void AddMultiple(string englishWord, IEnumerable<string> ukrainianTranslations);
}