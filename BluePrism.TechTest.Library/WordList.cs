using BluePrism.TechTest.Library.Interfaces;
using System.Collections.Concurrent;
using BluePrism.TechTest.Library.Extensions;
using System.IO.Abstractions;
using BluePrism.TechTest.Library.Exceptions;

namespace BluePrism.TechTest.Library
{
    public class WordList : IWordList
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        private readonly IFileSystem _fileSystem;

        public WordList(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)); ;
        }
        
        public async Task<string[]> ReadFromFileAsync(string path, int wordLength)
        {
            var words = new List<string>();

            try
            {
                using var streamReader = _fileSystem.File.OpenText(path);

                string? line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    if (line.IsValidWord(wordLength)) words.Add(line);
                }
            }
            catch(FileNotFoundException ex)
            {
                throw new DictionaryFileNotFoundException($"Unable to load dictionary file with name '{path}' at current location", ex);
            }

            return words.ToArray();
        }

        public string[] FindSimilarWords(string[] words, string word)
        {
            var similarWords = new ConcurrentBag<string>();

            int[] position = Enumerable.Range(0, word.Length).ToArray();

            Parallel.ForEach(position, new ParallelOptions { MaxDegreeOfParallelism = 4 }, index =>
            {
                char[] search = word.ToCharArray();

                Parallel.ForEach(Alphabet, new ParallelOptions { MaxDegreeOfParallelism = 2 }, letter =>
                {
                    search[index] = letter;

                    var searchWord = new string(search);

                    if (!searchWord.Equals(word, StringComparison.OrdinalIgnoreCase) 
                        && Array.BinarySearch(words, searchWord, StringComparer.OrdinalIgnoreCase) >= 0)
                    {
                        similarWords.Add(searchWord);
                    }
                });
            });

            return similarWords.ToArray();
        }
    }
}