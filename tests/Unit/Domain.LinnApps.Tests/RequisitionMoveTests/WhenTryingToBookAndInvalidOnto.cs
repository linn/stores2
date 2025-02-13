namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenTryingToBookAndInvalidOnto
    {
        private ReqMove sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new ReqMove(1, 1, 1, 1, null, null, null, string.Empty, string.Empty, string.Empty);
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.sut.OkToBook(TestTransDefs.LinnDeptToStock).Should().BeFalse();
        }
    }
}
