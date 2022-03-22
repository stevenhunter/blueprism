namespace BluePrism.TechTest.Library.Interfaces
{
    public interface IWordList
    {
        Task<string[]> ReadFromFileAsync(string path, int wordLength);

        string[] FindSimilarWords(string[] words, string word);
    }
}
