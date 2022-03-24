using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FluentAssertions;
using Xunit;

namespace BluePrism.TechTest.IntegrationTests;

[ExcludeFromCodeCoverage]
public class ConsoleAppTests
{
    private const string OutputFileName = "output.txt";
    private const string ApplicationExeName = "BluePrism.TechTest.Console.exe";

    [Theory]
    [InlineData("3-letter-words.txt", "bat", "lip", "Bat,Rat,Rap,Rip,Lip")]
    [InlineData("3-letter-words.txt", "Bat", "Lip", "Bat,Rat,Rap,Rip,Lip")]
    [InlineData("3-letter-words.txt", "eon", "sir", "")]
    [InlineData("4-letter-words.txt", "cake", "rate", "Cake,Make,Mate,Rate")]
    [InlineData("words-english.txt", "spin", "spot", "spin,spit,spot")]
    public void ConsoleApp_WhenInvoked_ProducesExpectedOutputAndReturnsZeroExitCode(
        string dictionaryFilename, string startWord, string endWord, string expectedOutput)
    {
        File.Delete(OutputFileName);

        var process = Process.Start(new ProcessStartInfo
        {
            FileName = ApplicationExeName,
            Arguments = $"-i {dictionaryFilename} -s {startWord} -e {endWord} -o {OutputFileName}"
        });
        process?.WaitForExit();

        var result = File.ReadAllLines(OutputFileName);

        process?.ExitCode.Should().Be(0);

        result.Should()
            .BeEquivalentTo(expectedOutput == string.Empty ? Array.Empty<string>() : expectedOutput.Split(','));
    }

    [Fact]
    public void ConsoleApp_WhenInvokedForNonExistingFile_ReturnsNonZeroExitCode()
    {
        File.Delete(OutputFileName);

        var process = Process.Start(new ProcessStartInfo
        {
            FileName = ApplicationExeName,
            Arguments = $"-i missing.txt -s start -e end -o {OutputFileName}"
        });
        process?.WaitForExit();

        process?.ExitCode.Should().NotBe(0);

        File.Exists(OutputFileName).Should().BeFalse();
    }
}