using BluePrism.TechTest.Library.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests
{
    [ExcludeFromCodeCoverage]
    public partial class WordListTests
    {
        [Fact]
        public void Ctor_GivenValidArguments_DoesNotThrow()
        {
            var act = new Action(() => _ = new WordList(Mock.Of<IFileManager>()));

            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_GivenNullFileManager_ThrowsArgumentNullException()
        {
            var act = new Action(() => _ = new WordList(null));

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'fileManager')");
        }
    }
}
