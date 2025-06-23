namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenTryingToBookAndMissingAllocation : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ReqMove(1, 1, 1, 1, null, null, null, string.Empty, string.Empty, string.Empty);
            this.ProcessResult = this.Sut.MoveCanBeBooked(TestTransDefs.StockToLinnDept);
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.ProcessResult.Success.Should().BeFalse();
            this.ProcessResult.Message.Should().Be("Move 0 on line 1 does not have a valid allocation.");
        }
    }
}
