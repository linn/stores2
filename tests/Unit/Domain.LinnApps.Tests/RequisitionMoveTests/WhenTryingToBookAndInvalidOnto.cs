using Linn.Stores2.TestData.Transactions;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;
    using FluentAssertions;

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
