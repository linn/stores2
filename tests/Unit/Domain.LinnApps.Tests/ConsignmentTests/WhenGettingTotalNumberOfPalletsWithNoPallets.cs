namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingTotalNumberOfPalletsWithNoPallets : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Pallets = null;
        }

        [Test]
        public void ShouldReturnZero()
        {
            this.Sut.TotalNumberOfPallets().Should().Be(0);
        }
    }
}
