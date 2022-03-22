using System;
using System.Linq;
using BluePrism.TechTest.Library.Extensions;
using FluentAssertions;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests.Extensions
{
    public class StringExtensions
    {
        [Theory]
        [InlineData("ab", "aa,ac,ab")]
        [InlineData("Ab", "Aa,Ac,Ab")]
        [InlineData("hh", "aa,bb,cc,dd,ee,ff,gg,hh")]
        public void FindWord_WhenWordIsInDictionary_DoesNotThrow(string word, string wordList)
        {
            var words = wordList.Split(',');
            words = words.OrderBy(w => w).ToArray();

            var func = new Func<bool>(() => word.FindWord(words, out var _));

            func.Should().NotThrow();
        }

        [Theory]
        [InlineData("ab", "aa,ac,ab")]
        [InlineData("Ab", "Aa,Ac,Ab")]
        [InlineData("pear", "apple,banana,grape,peach,pear,mango,pomegranate")]

        public void FindWord_WhenWordIsInDictionary_ReturnsExpected(string word, string wordList)
        {
            var words = wordList.Split(',');
            words = words.OrderBy(w => w).ToArray();

            word.FindWord(words, out var foundWord).Should().BeTrue();
            
            foundWord.Should().Be(word);
        }

        [Theory]
        [InlineData("sd", "aa,ac,ab")]
        [InlineData("Pd", "Aa,Ac,Ab")]
        [InlineData("strawberry", "apple,banana,grape,peach,pear,mango,pomegranate")]

        public void FindWord_WhenWordNotInDictionary_ReturnsExpected(string word, string wordList)
        {
            var words = wordList.Split(',');
            words = words.OrderBy(w => w).ToArray();

            word.FindWord(words, out var foundWord).Should().BeFalse();

            foundWord.Should().Be(string.Empty);
        }
    }
}
