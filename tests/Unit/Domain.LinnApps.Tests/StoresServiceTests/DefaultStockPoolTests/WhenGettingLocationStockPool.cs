namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.DefaultStockPoolTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingLocationStockPool : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.StockPoolResult = await this.Sut.DefaultStockPool(this.LocationWithDefaultStockPool.LocationId, null);
        }

        [Test]
        public void ShouldReturnCorrectStockPool()
        {
            this.StockPoolResult.StockPoolCode.Should().Be(this.StockPool.StockPoolCode);
        }
    }
}
