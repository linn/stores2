namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingTotalWeightWithNoPalletsOrItems : ContextBase
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
            this.Sut.TotalWeight().Should().Be(0m);
        }
    }
}
