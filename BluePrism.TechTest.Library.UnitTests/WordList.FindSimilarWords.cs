using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests
{
    public partial class WordListTests
    {
        [Theory]
        [InlineAutoMoqData("aa,bb,cc,dd", "dz", "dd")]
        [InlineAutoMoqData("aa,bb,cc,dd", "bt", "bb")]
        [InlineAutoMoqData("aa,bb,cc,dd", "sa", "aa")]
        [InlineAutoMoqData("aa,bb,cc,dd", "nc", "cc")]
        public void FindSimilarWords_WhenSingleMatchExists_ReturnsExpected(string dictionaryData, string word, string matchedWord, WordList sut)
        {
            var words = dictionaryData.Split(',');

            var result = sut.FindSimilarWords(words, word);

            result.Length.Should().Be(1);
            result[0].Should().Be(matchedWord);
        }

        [Theory]
        [InlineAutoMoqData("aaa,aab,aac,abb,abc", "aap", "aaa,aab,aac")]
        [InlineAutoMoqData("sssas,sssbs,ssscs,sssds,sasss", "sssqs", "sssas,sssbs,ssscs,sssds")]
        public void FindSimilarWords_WhenMultipleMatchesExist_ReturnsExpected(string dictionaryData, string word, string matchedWords, WordList sut)
        {
            var words = dictionaryData.Split(',');
            IEnumerable<string> expectedWords = matchedWords.Split(',');

            IEnumerable<string> result = sut.FindSimilarWords(words, word).OrderBy(s => s);

            result.Should().BeEquivalentTo(expectedWords);
        }

        [Theory]
        [InlineAutoMoqData("aa,bb,cc,dd", "ef")]
        [InlineAutoMoqData("aa,bb,cc,dd", "gh")]
        [InlineAutoMoqData("aa,bb,cc,dd", "ij")]
        [InlineAutoMoqData("aa,bb,cc,dd", "kl")]
        public void FindSimilarWords_WhenNoMatchExists_ReturnsEmptyArray(string dictionaryData, string word, WordList sut)
        {
            var words = dictionaryData.Split(',');

            var result = sut.FindSimilarWords(words, word);

            result.Should().BeEmpty();
        }
    }
}
