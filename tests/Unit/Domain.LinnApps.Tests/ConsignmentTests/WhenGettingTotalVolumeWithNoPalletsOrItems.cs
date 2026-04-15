namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingTotalVolumeWithNoPalletsOrItems : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = null;
            this.Sut.Pallets = null;
        }

        [Test]
        public void ShouldReturnZero()
        {
            this.Sut.TotalVolume().Should().Be(0m);
        }
    }
}
