namespace BluePrism.TechTest.Library.Interfaces
{
    public interface IPathFinder
    {
        IEnumerable<string> FindShortestPath(string[] words, string startWord, string endWord);
    }
}
