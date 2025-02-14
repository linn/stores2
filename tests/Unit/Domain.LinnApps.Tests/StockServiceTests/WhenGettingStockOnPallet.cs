namespace Linn.Stores2.Domain.LinnApps.Tests.StockServiceTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingStockOnPallet : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Results = this.Sut.GetStockLocators(this.PartNumber, null, this.PalletNumber).Result;
        }

        [Test]
        public void ShouldReturnBookedReq()
        {
            this.Results.Should().HaveCount(1);
            this.Results.Should().Contain(a => a.Id == 3);
        }
    }
}
