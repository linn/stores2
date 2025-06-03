namespace Linn.Stores2.Domain.LinnApps.Tests.StockServiceTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using NUnit.Framework;
    
    public class WhenCheckingValidStockLocationAndNoneInStockPool : ContextBase
    {
        private ProcessResult result;
    
        [SetUp]
        public async Task SetUp()
        {
            this.result = await this.Sut.ValidStockLocation(
                              this.LocationId,
                              null,
                              this.PartNumber,
                              5m,
                              "OK",
                              "OTHER STOCK POOL");
        }

        [Test]
        public void ShouldReturnError()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("Part P1 not at this location in stock pool OTHER STOCK POOL");
        }
    }
}
