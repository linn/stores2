namespace Linn.Stores2.Domain.LinnApps.Tests.StockServiceTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingStockAtLocation : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Results = this.Sut.GetStockLocators(this.PartNumber, this.LocationId, null).Result;
        }

        [Test]
        public void ShouldReturnBookedReq()
        {
            this.Results.Should().HaveCount(2);
            this.Results.Should().Contain(a => a.Id == 1);
            this.Results.Should().Contain(a => a.Id == 2);
        }
    }
}
