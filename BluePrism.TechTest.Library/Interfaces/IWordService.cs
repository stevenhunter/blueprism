namespace BluePrism.TechTest.Library.Interfaces;

public interface IWordService
{
    Task<string[]> FindShortestPathAsync(
        string inputFileName, string startWord, string endWord, string outputFileName);
}