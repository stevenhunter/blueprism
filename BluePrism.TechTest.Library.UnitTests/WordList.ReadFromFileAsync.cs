using AutoFixture.Xunit2;
using BluePrism.TechTest.Library.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests
{
    public partial class WordListTests
    {
        [Theory, AutoMoqData]
        public async Task ReadFromFileAsync_WithEmptyFilePath_ThrowsException(
            [Frozen] Mock<IFileSystem> fileSystemMock, int wordLength, WordList sut)
        {
            fileSystemMock.Setup(fs => fs.File.OpenText(string.Empty)).Throws<FileNotFoundException>()
                .Verifiable();

            var func = new Func<Task>(async () => await sut.ReadFromFileAsync(string.Empty, wordLength));

            await func.Should().ThrowAsync<DictionaryFileNotFoundException>()
                .WithMessage("Unable to load dictionary file with name '' at current location");

            fileSystemMock.Verify(fs => fs.File.OpenText(string.Empty));
        }

        [Theory, AutoMoqData]
        public async Task ReadFromFileAsync_WithNonExistingFilePath_ThrowsException(
            string path, int wordLength, [Frozen] Mock<IFileSystem> fileSystemMock, WordList sut)
        {
            fileSystemMock.Setup(fs => fs.File.OpenText(path)).Throws<FileNotFoundException>()
                .Verifiable();

            var func = new Func<Task>(async () => await sut.ReadFromFileAsync(path, wordLength));

            await func.Should().ThrowAsync<DictionaryFileNotFoundException>()
                .WithMessage($"Unable to load dictionary file with name '{path}' at current location");

            fileSystemMock.Verify(fs => fs.File.OpenText(path));
        }

        [Theory, AutoMoqData]
        public async Task ReadFromFileAsync_WithData_DoesNotThrow(
            string path, [Frozen] Mock<IFileSystem> fileSystemMock, WordList sut)
        {
            var text = "apple\ngrape\nmango";
            
            var byteArray = Encoding.UTF8.GetBytes(text);
            
            var memoryStream = new MemoryStream(byteArray);

            fileSystemMock.Setup(fm => fm.File.OpenText(It.IsAny<string>()))
                .Returns(() => new StreamReader(memoryStream))
                .Verifiable();

            var func = new Func<Task>(async () => await sut.ReadFromFileAsync(path, 5));

            await func.Should().NotThrowAsync();

            fileSystemMock.Verify(fs => fs.File.OpenText(path));
        }
    }
}
