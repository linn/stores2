namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentPalletTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenCalculatingVolumeWithNullDimension : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Height = 10;
            this.Sut.Width = null;
            this.Sut.Depth = 30;
        }

        [Test]
        public void ShouldReturnNull()
        {
            this.Sut.Volume.Should().BeNull();
        }
    }
}
