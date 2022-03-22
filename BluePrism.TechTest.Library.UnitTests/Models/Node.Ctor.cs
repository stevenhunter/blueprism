using Xunit;
using System;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using BluePrism.TechTest.Library.Models;
using AutoFixture.Xunit2;

namespace BluePrism.TechTest.Library.UnitTests.Models
{
    [ExcludeFromCodeCoverage]
    public class NodeTests
    {
        [Theory, AutoData]
        public void Ctor_GivenValidArguments_DoesNotThrow(string word)
        {
            Node? node = null;

            var func = new Func<Node>(() => node = new Node(word, null));

            func.Should().NotThrow();

            node?.Word.Should().Be(word);
            node?.Parent.Should().BeNull();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_GivenInvalidWord_ThrowsArgumentNullException(string word)
        {
            Action act = () => _ = new Node(word, null);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null, empty or whitespace (Parameter 'word')");
        }
    }
}