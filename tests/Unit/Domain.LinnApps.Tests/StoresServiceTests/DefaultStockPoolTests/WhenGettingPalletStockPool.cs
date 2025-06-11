namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.DefaultStockPoolTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingPalletStockPool : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.StockPoolResult = await this.Sut.DefaultStockPool(null, this.PalletWithDefaultStockPool.PalletNumber);
        }

        [Test]
        public void ShouldReturnCorrectStockPool()
        {
            this.StockPoolResult.StockPoolCode.Should().Be(this.StockPool.StockPoolCode);
        }
    }
}
