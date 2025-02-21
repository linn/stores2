namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidStockPoolTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenStockPoolPartDataNotSupplied : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = null;

            this.Result = this.Sut.ValidStockPool(this.Part, this.StockPool);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("Incomplete stock pool or part data supplied");
        }
    }
}
