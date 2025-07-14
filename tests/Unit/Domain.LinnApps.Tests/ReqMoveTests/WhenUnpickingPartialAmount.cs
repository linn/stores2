using FluentAssertions;

namespace Linn.Stores2.Domain.LinnApps.Tests.ReqMoveTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Requisitions;
    using NUnit.Framework;

    public class WhenUnpickingPartialAmount
    {
        private ReqMove sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new MoveWithLocation(
                new StockLocator { StorageLocation = new StorageLocation { LocationCode = "E-SMT-3" } },
                new StorageLocation { LocationCode = "E-K2-34", LocationId = 4989 },
                10);

            this.sut.UnpickQuantity(4);
        }

        [Test]
        public void ShouldReduceQty()
        {
            this.sut.IsCancelled().Should().BeFalse();
            this.sut.Quantity.Should().Be(6);
        }
    }
}
