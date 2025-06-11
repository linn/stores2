namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.DefaultStockPoolTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingDefaultStockPool : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.StockPoolResult = await this.Sut.DefaultStockPool(null, null);
        }

        [Test]
        public void ShouldReturnDefaultStockPool()
        {
            this.StockPoolResult.StockPoolCode.Should().Be(this.DefaultStockPool.StockPoolCode);
        }
    }
}
