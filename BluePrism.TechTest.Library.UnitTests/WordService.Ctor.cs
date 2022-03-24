using System;
using System.Diagnostics.CodeAnalysis;
using BluePrism.TechTest.Library.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests;

[ExcludeFromCodeCoverage]
public partial class WordServiceTests
{
    [Fact]
    public void Ctor_GivenValidArguments_DoesNotThrow()
    {
        var act = new Action(() => _ = new WordService(
            Mock.Of<IWordRepository>(), Mock.Of<IOutputWriter>()));

        act.Should().NotThrow();
    }

    [Fact]
    public void Ctor_GivenNullOutputWriter_ThrowsArgumentNullException()
    {
        var act = new Action(() => _ = new WordService(
            Mock.Of<IWordRepository>(), null!));

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'outputWriter')");
    }

    [Fact]
    public void Ctor_GivenNullWordRepository_ThrowsArgumentNullException()
    {
        var act = new Action(() => _ = new WordService(
            null!, Mock.Of<IOutputWriter>()));

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'wordRepository')");
    }
}