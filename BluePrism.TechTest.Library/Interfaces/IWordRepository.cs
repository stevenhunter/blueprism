namespace BluePrism.TechTest.Library.Interfaces;

public interface IWordRepository
{
    string[] Words { get; }

    Task LoadFromFileAsync(string path, int wordLength);
}