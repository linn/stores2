namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentPalletTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenCalculatingVolume : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Height = 10;
            this.Sut.Width = 20;
            this.Sut.Depth = 30;
        }

        [Test]
        public void ShouldReturnHeightTimesWidthTimesDepth()
        {
            this.Sut.Volume.Should().Be(6000);
        }
    }
}
