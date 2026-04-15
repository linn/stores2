namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentPalletTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingPalletDimensions : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Height = 10;
            this.Sut.Width = 20;
            this.Sut.Depth = 30;
        }

        [Test]
        public void ShouldReturnDimensionsString()
        {
            this.Sut.PalletDimensions.Should().Be("10 x 20 x 30 cm");
        }
    }
}
