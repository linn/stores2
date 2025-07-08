namespace Linn.Stores2.Domain.LinnApps.Tests.ReqMoveTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Requisitions;
    using NUnit.Framework;

    public class WhenUnpickingFullAmount
    {
        private ReqMove sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new MoveWithLocation(
                new StockLocator { StorageLocation = new StorageLocation { LocationCode = "E-SMT-3" } },
                new StorageLocation { LocationCode = "E-K2-34", LocationId = 4989 },
                10);

            this.sut.UnpickQuantity(10);
        }

        [Test]
        public void ShouldCancelMove()
        {
            this.sut.IsCancelled().Should().BeTrue();
        }
    }
}
