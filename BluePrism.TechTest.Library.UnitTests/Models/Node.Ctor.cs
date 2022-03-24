using FluentAssertions;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
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
            var act = new Action(() => _ = new Node(word));

            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_GivenNullWord_ThrowsArgumentNullException()
        {
            var act = new Action(() => _ = new Node(null!));

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'word')");
        }
    }
}
