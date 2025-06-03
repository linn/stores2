namespace Linn.Stores2.Domain.LinnApps.Tests.StockServiceTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Tests.Extensions;

    using NUnit.Framework;
    
    public class WhenCheckingValidStockLocationAndNotEnoughStockInStockPool : ContextBase
    {
        private ProcessResult result;
    
        [SetUp]
        public async Task SetUp()
        {
            this.DbContext.StockLocators.AddAndSave(
                this.DbContext,
                new StockLocator
                    {
                        Id = 15,
                        LocationId = this.LocationId,
                        PartNumber = this.PartNumber,
                        State = "OK",
                        Category = "C1",
                        CurrentStock = "Y",
                        Quantity = 2,
                        QuantityAllocated = 0,
                        StockPoolCode = "OTHER STOCK POOL"
                });

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
            this.result.Message.Should().Be("Not enough stock at this location, unallocated qty: 2");
        }
    }
}
