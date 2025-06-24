namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenCheckingStockPickedAndFullyAllocated
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(1, 1, TestParts.Cap003, 1, TestTransDefs.StockToLinnDept);
            var move = new ReqMove(1, 1, 1, 1, 1, null, 1, "LINN", "STORES", "FREE", 
                new StockLocator { Id = 1, BatchRef = "BUNNY", Quantity = 10, QuantityAllocated = 1 });
            this.sut.Moves.Add(move);
        }

        [Test]
        public void ShouldHaveStockPicked()
        {
            this.sut.StockPicked().Should().BeTrue();
        }
    }
}
