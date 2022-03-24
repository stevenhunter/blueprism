using AutoFixture.Xunit2;

namespace BluePrism.TechTest.Library.UnitTests;

public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoMoqDataAttribute(), objects)
    {
    }
}