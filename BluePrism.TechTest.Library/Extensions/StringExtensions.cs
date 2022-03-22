using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using BluePrism.TechTest.Library.Models;

namespace BluePrism.TechTest.Library.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidWord(this string word, int length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "Supplied length must be greater than 0");
            
            if (string.IsNullOrWhiteSpace(word)) return false;

            var pattern = $"^[A-Za-z]{{1}}[a-z]{{{length - 1}}}$";

            return Regex.IsMatch(word, pattern);
        }

        public static bool FindWord(this string word, string[] words, out string foundWord)
        {
            var foundAtIndex = Array.BinarySearch(words, word, StringComparer.OrdinalIgnoreCase);

            foundWord = foundAtIndex >= 0 ? words[foundAtIndex] : string.Empty;

            return foundAtIndex >= 0;
        }

        public static string[] FindSimilarWords(this string word, string[] words)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            var similarWords = new ConcurrentBag<string>();

            var position = Enumerable.Range(0, word.Length).ToArray();

            Parallel.ForEach(position, new ParallelOptions { MaxDegreeOfParallelism = 4 }, index =>
            {
                var search = word.ToCharArray();

                Parallel.ForEach(alphabet, new ParallelOptions { MaxDegreeOfParallelism = 2 }, letter =>
                {
                    search[index] = letter;

                    var searchWord = new string(search);

                    if (searchWord.Equals(word, StringComparison.OrdinalIgnoreCase)) return;

                    var found = searchWord.FindWord(words, out var foundWord);

                    if (found) similarWords.Add(foundWord);
                });
            });

            return similarWords.ToArray();
        }

        public static string[] FindShortestPath(this string[] words, string startWord, string endWord)
        {
            var visited = new List<Node>();
            var queue = new Queue<Node>();
            queue.Enqueue(new Node(startWord));

            while (queue.Any())
            {
                var parentNode = queue.Dequeue();
                visited.Add(parentNode);

                var similarWords = parentNode.Word.FindSimilarWords(words);

                foreach (var similarWord in similarWords)
                {
                    var node = new Node(similarWord) { Parent = parentNode };

                    if (similarWord.Equals(endWord, StringComparison.OrdinalIgnoreCase))
                        return GetWalkedPath(node).Reverse().ToArray();

                    if (!visited.Any(v => v.Word.Equals(similarWord, StringComparison.OrdinalIgnoreCase)))
                        queue.Enqueue(node);
                }
            }

            return Array.Empty<string>();
        }

        private static IEnumerable<string> GetWalkedPath(Node endNode)
        {
            var node = endNode;

            while (node != null)
            {
                yield return node.Word;

                node = node.Parent;
            }
        }
    }
}
