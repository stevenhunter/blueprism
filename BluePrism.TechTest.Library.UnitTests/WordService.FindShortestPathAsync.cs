using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using BluePrism.TechTest.Library.Exceptions;
using BluePrism.TechTest.Library.Interfaces;
using Moq;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests
{
    public partial class WordServiceTests
    {
        [Theory]
        [InlineAutoMoqData("bad,bed,fed,fad,far")]
        [InlineAutoMoqData("boat,coat,cost,most,moat")]
        public async Task FindShortestPath_WithValidArguments_IncludingOutputFileName_DoesNotThrow(
            string wordList, string inputFileName, string outputFileName,
            [Frozen] Mock<IWordRepository> wordRepositoryMock, [Frozen] Mock<IOutputWriter> outputWriterMock,
            WordService sut) 
        {
            var words = wordList.Split(',').OrderBy(s => s).ToArray();

            wordRepositoryMock.SetupGet(wr => wr.Words).Returns(words);

            var func = new Func<Task>(async () => await sut.FindShortestPathAsync(inputFileName,
                words.First(), words.Last(), outputFileName));

            await func.Should().NotThrowAsync();

            wordRepositoryMock.Verify(wr =>
                wr.LoadFromFileAsync(inputFileName, words.First().Length), Times.Once);

            wordRepositoryMock.Verify(wr => wr.Words, Times.Exactly(2));

            outputWriterMock.Verify(ow =>
                ow.WriteToFileAsync(outputFileName, It.IsAny<string[]>()), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData("bad,bed,fed,fad,far")]
        [InlineAutoMoqData("boat,coat,cost,most,moat")]
        public async Task FindShortestPath_WithValidArguments_NotIncludingOutputFileName_DoesNotThrow(
            string wordList, string inputFileName,
            [Frozen] Mock<IWordRepository> wordRepositoryMock, [Frozen] Mock<IOutputWriter> outputWriterMock,
            WordService sut)
        {
            var words = wordList.Split(',').OrderBy(s => s).ToArray();

            wordRepositoryMock.Setup(wr => wr.Words).Returns(words);

            var func = new Func<Task>(async () => await sut.FindShortestPathAsync(inputFileName,
                words.First(), words.Last(), string.Empty));

            await func.Should().NotThrowAsync();

            wordRepositoryMock.Verify(wr =>
                wr.LoadFromFileAsync(inputFileName, words.First().Length), Times.Once);

            wordRepositoryMock.Verify(wr => wr.Words, Times.Exactly(2));

            outputWriterMock.Verify(ow =>
                ow.WriteToFileAsync(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
        }

        [Theory]
        [InlineAutoMoqData("bad,bed,fed,fad,far", "lap")]
        [InlineAutoMoqData("boat,coat,cost,most,moat", "tree")]
        public async Task FindShortestPath_WithInvalidStartWord_ThrowsException(
            string wordList, string startWord, string inputFileName,
            [Frozen] Mock<IWordRepository> wordRepositoryMock, [Frozen] Mock<IOutputWriter> outputWriterMock,
            WordService sut)
        {
            var words = wordList.Split(',');

            wordRepositoryMock.Setup(wr => wr.Words).Returns(words);

            var func = new Func<Task>(async () => await sut.FindShortestPathAsync(inputFileName,
                startWord, words.Last(), string.Empty));

            await func.Should().ThrowAsync<WordNotFoundInDictionaryException>()
                .WithMessage("Start word not found in dictionary");

            wordRepositoryMock.Verify(wr =>
                wr.LoadFromFileAsync(inputFileName, It.IsAny<int>()), Times.Once);

            wordRepositoryMock.Verify(wr => wr.Words, Times.Once);

            outputWriterMock.Verify(ow =>
                ow.WriteToFileAsync(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
        }

        [Theory]
        [InlineAutoMoqData("bad,bed,fed,fad,far", "lap")]
        [InlineAutoMoqData("boat,coat,cost,most,moat", "tree")]
        public async Task FindShortestPath_WithInvalidEndWord_ThrowsException(
            string wordList, string endWord, string inputFileName,
            [Frozen] Mock<IWordRepository> wordRepositoryMock, [Frozen] Mock<IOutputWriter> outputWriterMock,
            WordService sut)
        {
            var words = wordList.Split(',');
            
            wordRepositoryMock.Setup(wr => wr.Words).Returns(words);

            var func = new Func<Task>(async () => await sut.FindShortestPathAsync(inputFileName,
                words.First(), endWord, string.Empty));

            await func.Should().ThrowAsync<WordNotFoundInDictionaryException>()
                .WithMessage("End word not found in dictionary");

            wordRepositoryMock.Verify(wr =>
                wr.LoadFromFileAsync(inputFileName, It.IsAny<int>()), Times.Once);

            wordRepositoryMock.Verify(wr => wr.Words, Times.Once);

            outputWriterMock.Verify(ow =>
                ow.WriteToFileAsync(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
        }
    }
}
