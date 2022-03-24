using System.IO.Abstractions;
using BluePrism.TechTest.Library.Exceptions;
using BluePrism.TechTest.Library.Extensions;
using BluePrism.TechTest.Library.Interfaces;

namespace BluePrism.TechTest.Library;

public class WordRepository : IWordRepository
{
    private readonly IFileSystem _fileSystem;

    public WordRepository(IFileSystem fileSystem)
    {
        ArgumentNullException.ThrowIfNull(fileSystem);

        _fileSystem = fileSystem;
    }

    public string[] Words { get; private set; } = Array.Empty<string>();

    public async Task LoadFromFileAsync(string path, int wordLength)
    {
        var words = new List<string>();
        StreamReader? streamReader = null;
        try
        {
            try
            {
                streamReader = _fileSystem.File.OpenText(path);
            }
            catch (Exception ex)
            {
                throw new DictionaryFileNotFoundException(
                    $"Unable to load dictionary file with name '{path}' at current location", ex);
            }

            string? line;
            while ((line = await streamReader.ReadLineAsync()) != null)
                if (line.IsValidWord(wordLength))
                    words.Add(line);
        }
        finally
        {
            streamReader?.Close();
            streamReader?.Dispose();
        }

        Words = words.OrderBy(s => s).ToArray();
    }
}