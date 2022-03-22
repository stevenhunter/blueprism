using System;
using Xunit;
using BluePrism.TechTest.Library.Extensions;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace BluePrism.TechTest.Library.UnitTests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        public void IsValidWord_WhenPermittedLengthLessThanZero_ThrowsException(int length)
        {
            var word = "test";

            var func = new Func<bool>(() => _ = word.IsValidWord(length));

            func.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Supplied length must be greater than 0 (Parameter 'length')");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(999)]
        public void IsValidWord_WhenPermittedLengthGreaterThanZero_DoesNotThrow(int length)
        {
            var word = "test";

            var func = new Func<bool>(() => _ = word.IsValidWord(length));

            func.Should().NotThrow();
        }

        [Fact]
        public void IsValidWord_WhenWordIsEmptyString_ReturnsFalse()
        {
            var word = string.Empty;
            bool result = false;

            var func = new Func<bool>(() => result = word.IsValidWord(5));

            func.Should().NotThrow();
            
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValidWord_WhenWordIsWhitespace_ReturnsFalse()
        {
            var word = " ";
            bool result = false;

            var func = new Func<bool>(() => result = word.IsValidWord(5));

            func.Should().NotThrow();

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(4, "apple")]
        [InlineData(8, "pomegranate")]
        [InlineData(2, "kiwi")]
        [InlineData(9, "strawberry")]
        public void IsValidWord_WhenWordLengthGreaterThanPermitted_ReturnsFalse(int permittedWordLength, string word)
        {
            bool result = false; 
            
            var func = new Func<bool>(() => result = word.IsValidWord(permittedWordLength));

            func.Should().NotThrow();

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(3, "kiwi")]
        [InlineData(4, "\\9th")]
        [InlineData(4, "12th")]
        [InlineData(2, "AA")]
        [InlineData(3, "A&B")]
        [InlineData(5, "Ab cd")]
        [InlineData(5, "Ab Cd")]
        [InlineData(5, "ab cd")]
        [InlineData(5, " abcd")]
        [InlineData(5, "abcd ")]
        [InlineData(4, " abcd ")]
        [InlineData(5, " abcd ")]
        public void IsValidWord_WhenNotValidMatch_ReturnsFalse(int permittedWordLength, string word)
        {
            bool result = false;

            var func = new Func<bool>(() => result = word.IsValidWord(permittedWordLength));

            func.Should().NotThrow();

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(4, "kiwi")]
        [InlineData(4, "Kiwi")]
        [InlineData(11, "pomegranate")]
        public void IsValidWord_WhenValidMatch_ReturnsTrue(int permittedWordLength, string word)
        {
            bool result = false;

            var func = new Func<bool>(() => result = word.IsValidWord(permittedWordLength));

            func.Should().NotThrow();

            result.Should().BeTrue();
        }
    }
}
