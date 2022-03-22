using BluePrism.TechTest.Library.Exceptions;
using BluePrism.TechTest.Library.Interfaces;
using BluePrism.TechTest.Library.Extensions;

namespace BluePrism.TechTest.Library
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly IOutputWriter _outputWriter;

        public WordService(IWordRepository wordRepository, IOutputWriter outputWriter)
        {
            ArgumentNullException.ThrowIfNull(wordRepository);
            ArgumentNullException.ThrowIfNull(outputWriter);

            _wordRepository = wordRepository;
            _outputWriter = outputWriter;
        }
        
        public async Task<string[]> FindShortestPathAsync(
            string inputFileName, string startWord, string endWord, string outputFileName)
        {
            await _wordRepository.LoadFromFileAsync(inputFileName, startWord.Length);
            
            ValidateStartAndEndWords(_wordRepository.Words, ref startWord, ref endWord);

            var shortestPath = _wordRepository.Words.FindShortestPath(startWord, endWord);

            if(!string.IsNullOrEmpty(outputFileName))
                await _outputWriter.WriteToFileAsync(outputFileName, shortestPath.ToArray());

            return shortestPath;
        }

        private static void ValidateStartAndEndWords(
            string[] words, ref string startWord, ref string endWord)
        {
            if (!startWord.FindWord(words, out startWord))
                throw new WordNotFoundInDictionaryException(
                    "Start word not found in dictionary");
            
            if (!endWord.FindWord(words, out endWord))
                throw new WordNotFoundInDictionaryException(
                    "End word not found in dictionary");
        }
    }
}
