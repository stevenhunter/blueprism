using AutoFixture.Xunit2;
using BluePrism.TechTest.Library.Exceptions;
using FluentAssertions;
using System;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests
{
    public partial class WordRepositoryTests
    {
        [Theory, AutoData]
        public async Task ReadFromFileAsync_WithEmptyFilePath_ThrowsException(
            MockFileSystem fileSystemMock, int wordLength)
        {
            var sut = new WordRepository(fileSystemMock);

            var func = new Func<Task>(async () => await sut.LoadFromFileAsync(string.Empty, wordLength));

            await func.Should().ThrowAsync<DictionaryFileNotFoundException>()
                .WithMessage("Unable to load dictionary file with name '' at current location");
        }

        [Theory, AutoData]
        public async Task ReadFromFileAsync_WithNonExistingFilePath_ThrowsException(
            string path, int wordLength, MockFileSystem fileSystemMock)
        {
            var sut = new WordRepository(fileSystemMock);

            var func = new Func<Task>(async () => await sut.LoadFromFileAsync(path, wordLength));

            await func.Should().ThrowAsync<DictionaryFileNotFoundException>()
                .WithMessage($"Unable to load dictionary file with name '{path}' at current location");
        }

        [Theory, AutoData]
        public async Task ReadFromFileAsync_WithData_DoesNotThrow(
            string path, [Frozen] MockFileSystem fileSystemMock)
        {
            var text = "apple\ngrape\nmango";

            fileSystemMock.AddFile(path, new MockFileData(text));
            
            var sut = new WordRepository(fileSystemMock);

            var fileData = Array.Empty<string>();

            var func = new Func<Task>(async () => await sut.LoadFromFileAsync(path, 5));

            await func.Should().NotThrowAsync();

            sut.Words.Should().BeEquivalentTo(text.Split('\n'));
        }
    }
}
