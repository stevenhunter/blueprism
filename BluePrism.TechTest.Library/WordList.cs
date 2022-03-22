using BluePrism.TechTest.Library.Exceptions;
using BluePrism.TechTest.Library.Interfaces;
using System.Collections.Concurrent;
using BluePrism.TechTest.Library.Extensions;

namespace BluePrism.TechTest.Library
{
    public class WordList : IWordList
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        private readonly IFileManager _fileManager;

        public WordList(IFileManager fileManager)
        {
            _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager)); ;
        }
        
        public async Task<string[]> ReadFromFileAsync(string path, int wordLength)
        {
            var words = new List<string>();
            
            ValidateFilePath(path);

            using var streamReader = _fileManager.StreamReader(path);
            
            string? line;
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                if (line.IsValidWord(wordLength)) words.Add(line);
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

        private void ValidateFilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) 
                throw new ArgumentOutOfRangeException(nameof(path));

            if (!_fileManager.FileExists(path)) 
                throw new DictionaryFileNotFoundException($"File not found at path: {path}");
        }
    }
}