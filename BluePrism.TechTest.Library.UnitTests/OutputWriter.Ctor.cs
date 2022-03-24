using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace BluePrism.TechTest.Library.UnitTests;

[ExcludeFromCodeCoverage]
public partial class OutputWriterTests
{
    [Fact]
    public void Ctor_GivenValidArguments_DoesNotThrow()
    {
        var act = new Action(() => _ = new OutputWriter(Mock.Of<IFileSystem>()));

        act.Should().NotThrow();
    }

    [Fact]
    public void Ctor_GivenNullFileSystem_ThrowsArgumentNullException()
    {
        var act = new Action(() => _ = new OutputWriter(null!));

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'fileSystem')");
    }
}