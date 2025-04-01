namespace Linn.Stores2.Domain.LinnApps.Tests.StockServiceTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using NUnit.Framework;
    
    public class WhenCheckingValidStockLocationAndValid : ContextBase
    {
        private ProcessResult result;
    
        [SetUp]
        public async Task SetUp()
        {
            this.result = await this.Sut.ValidStockLocation(null, 900, "2STATES", 100m, "QC");
        }

        [Test]
        public void ShouldReturnError()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
