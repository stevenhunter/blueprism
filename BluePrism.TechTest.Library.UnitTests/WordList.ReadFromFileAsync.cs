using AutoFixture.Xunit2;
using BluePrism.TechTest.Library.Exceptions;
using BluePrism.TechTest.Library.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests
{
    public partial class WordListTests
    {
        [Theory, AutoMoqData]
        public async Task ReadFromFileAsync_WithEmptyFilePath_ThrowsException(int wordLength, WordList sut)
        {
            var func = new Func<Task>(async () => await sut.ReadFromFileAsync(string.Empty, wordLength));

            await func.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("Specified argument was out of the range of valid values. (Parameter 'path')");
        }

        [Theory, AutoMoqData]
        public async Task ReadFromFileAsync_WithNonExistingFilePath_ThrowsException(int wordLength, WordList sut)
        {
            var func = new Func<Task>(async () => await sut.ReadFromFileAsync("not_a_file.txt", wordLength));

            await func.Should().ThrowAsync<DictionaryFileNotFoundException>()
                .WithMessage("File not found at path: not_a_file.txt");
        }

        [Theory, AutoMoqData]
        public async Task ReadFromFileAsync_WithData_DoesNotThrow(
            string path, [Frozen] Mock<IFileManager> fileManagerMock, WordList sut)
        {
            var text = "apple\ngrape\nmango";
            
            var byteArray = Encoding.UTF8.GetBytes(text);
            
            var memoryStream = new MemoryStream(byteArray);

            fileManagerMock.Setup(fm => fm.FileExists(path))
                .Returns(true)
                .Verifiable();

            fileManagerMock.Setup(fm => fm.StreamReader(path))
                .Returns(() => new StreamReader(memoryStream))
                .Verifiable();

            var func = new Func<Task>(async () => await sut.ReadFromFileAsync(path, 5));

            await func.Should().NotThrowAsync();

            fileManagerMock.VerifyAll();
        }
    }
}
