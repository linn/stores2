namespace Linn.Stores2.Domain.LinnApps.Tests.StockServiceTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using NUnit.Framework;
    
    public class WhenCheckingValidStockLocationAndNotEnoughStockAtLocation : ContextBase
    {
        private ProcessResult result;
    
        [SetUp]
        public async Task SetUp()
        {
            this.result = await this.Sut.ValidStockLocation(null, 900, "2STATES", 500m, "QC");
        }

        [Test]
        public void ShouldReturnError()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("Not enough stock at this location, unallocated qty: 100");
        }
    }
}
