using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace BluePrism.TechTest.Library.UnitTests;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(() => 
        new Fixture().Customize(new CompositeCustomization(
        new AutoMoqCustomization())))
    {
    }
}