using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2;
using BluePrism.TechTest.Library.Extensions;
using FluentAssertions;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests.Extensions;

public partial class StringExtensionsTests
{
    [Theory]
    [InlineAutoData("ab", "bd", "ab,ac,bc,bd")]
    [InlineAutoData("bd", "ab", "bd,bc,ac,ab")]
    public void FindShortestPath_WhenSinglePathsExists_ReturnsExpected(
        string startWord, string endWord, string expectedPath)
    {
        var words = GetSortedWordList(startWord.Length);

        var result = Array.Empty<string>();

        var func = new Func<string[]>(() => result = words.FindShortestPath(startWord, endWord));

        func.Should().NotThrow();

        result.Should().BeEquivalentTo(expectedPath.Split(','));
    }

    [Theory]
    [InlineAutoData("bad", "far", "bad,fad,far")]
    [InlineAutoData("boat", "moat", "boat,moat")]
    [InlineAutoData("boat", "most", "boat,moat,most")]
    public void FindShortestPath_WhenDifferentLengthPathsExist_ReturnsShortest(
        string startWord, string endWord, string expectedPath)
    {
        var words = GetSortedWordList(startWord.Length);

        var result = Array.Empty<string>();

        var func = new Func<string[]>(() => result = words.FindShortestPath(startWord, endWord));

        func.Should().NotThrow();

        result.Should().BeEquivalentTo(expectedPath.Split(','));
    }

    [Theory]
    [InlineAutoData("moat", "cost", "moat,most,cost", "moat,coat,cost")]
    [InlineAutoData("most", "coat", "most,cost,coat", "most,moat,coat")]
    public void FindShortestPath_WhenMoreThanOneEqualLengthPathsExist_ReturnsEither(
        string startWord, string endWord, string expectedPath1, string expectedPath2)
    {
        var words = GetSortedWordList(startWord.Length);

        var path = Array.Empty<string>();

        var func = new Func<string[]>(() => path = words.FindShortestPath(startWord, endWord));

        func.Should().NotThrow();

        var result = path.SequenceEqual(expectedPath1.Split(','))
                     || path.SequenceEqual(expectedPath2.Split(','));

        result.Should().BeTrue($"Result was not one of expected paths {expectedPath1} or {expectedPath2}");
    }

    [Fact]
    public void FindShortestPath_WhenNoPathExists_ReturnsEmptyResult()
    {
        var words = GetSortedWordList(2);

        var result = Array.Empty<string>();

        var func = new Func<string[]>(() => result = words.FindShortestPath("ab", "zz"));

        func.Should().NotThrow();

        result.Should().BeEquivalentTo(Array.Empty<string>());
    }

    private static string[] GetSortedWordList(int length)
    {
        var words = GetWordList(length);
        return words.OrderBy(w => w).ToArray();
    }

    private static IEnumerable<string> GetWordList(int length)
    {
        return length switch
        {
            2 => new[] {"ab", "ac", "bc", "bd", "xx", "ss"},
            3 => new[] {"bad", "bed", "fed", "fad", "far"},
            4 => new[] {"boat", "coat", "cost", "most", "moat"},
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}