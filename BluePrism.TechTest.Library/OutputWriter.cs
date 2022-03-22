using BluePrism.TechTest.Library.Interfaces;
using System.IO.Abstractions;

namespace BluePrism.TechTest.Library
{
    public class OutputWriter : IOutputWriter
    {
        private readonly IFileSystem _fileSystem;

        public OutputWriter(IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);

            _fileSystem = fileSystem;
        }

        public async Task WriteToFileAsync(string path, IEnumerable<string> output)
        {
            await _fileSystem.File.WriteAllLinesAsync(path, output);
        }
    }
}
