using BluePrism.TechTest.Library.Interfaces;
using BluePrism.TechTest.Library.Models;

namespace BluePrism.TechTest.Library
{
    public class PathFinder: IPathFinder
    {
        public readonly IWordList _wordDictionary;
        
        public PathFinder(IWordList wordDictionary)
        {
            _wordDictionary = wordDictionary;
        }

        public IEnumerable<string> FindShortestPath(string[] words, string startWord, string endWord)
        {
            var visited = new List<Node>();
            var queue = new Queue<Node>();
            queue.Enqueue(new Node(startWord, null));

            while (queue.Any())
            {
                var parentNode = queue.Dequeue();
                visited.Add(parentNode);

                foreach (var similarWord in _wordDictionary.FindSimilarWords(words, parentNode.Word))
                {
                    var node = new Node(similarWord, parentNode);

                    if (similarWord.Equals(endWord, StringComparison.OrdinalIgnoreCase)) 
                        return GetWalkedPath(node).Reverse();

                    if (!visited.Any(v => v.Word.Equals(similarWord, StringComparison.OrdinalIgnoreCase)))
                        queue.Enqueue(node);
                }
            }

            return Array.Empty<string>();
        }

        private static IEnumerable<string> GetWalkedPath(Node endNode)
        {
            Node? node = endNode;

            while (node != null)
            {
                yield return node.Word;
                node = node.Parent;
            }
        }
    }
}
