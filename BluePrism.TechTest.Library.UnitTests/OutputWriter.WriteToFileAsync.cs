using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests
{
    public partial class OutputWriterTests
    {
        [Theory, AutoData]
        public async Task WriteToFile_DoesNotThrow(string[] data, MockFileSystem fileSystemMock)
        {
            const string path = "output.txt";

            var sut = new OutputWriter(fileSystemMock);

            var func = new Func<Task>(async () => await sut.WriteToFileAsync(path, data));

            await func.Should().NotThrowAsync();

            fileSystemMock.FileExists(path).Should().BeTrue();
        }
    }
}
