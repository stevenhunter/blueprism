using BluePrism.TechTest.Library.Exceptions;
using BluePrism.TechTest.Library.Extensions;
using BluePrism.TechTest.Library.Interfaces;

namespace BluePrism.TechTest.Library;

public class WordService : IWordService
{
    private readonly IOutputWriter _outputWriter;
    private readonly IWordRepository _wordRepository;

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

        if (!string.IsNullOrEmpty(outputFileName))
            await _outputWriter.WriteToFileAsync(outputFileName, shortestPath.ToArray());

        return shortestPath;
    }

    private static void ValidateStartAndEndWords(
        string[] words, ref string startWord, ref string endWord)
    {
        if (startWord.Length != endWord.Length)
            throw new ArgumentException("Start and end words must be of equal length");

        if (!startWord.IsValidWord(startWord.Length))
            throw new InvalidWordException("Start word is not a valid word");

        if (!endWord.IsValidWord(endWord.Length))
            throw new InvalidWordException("End word is not a valid word");

        if (!startWord.FindWord(words, out startWord))
            throw new WordNotFoundInDictionaryException(
                "Start word not found in dictionary");

        if (!endWord.FindWord(words, out endWord))
            throw new WordNotFoundInDictionaryException(
                "End word not found in dictionary");
    }
}