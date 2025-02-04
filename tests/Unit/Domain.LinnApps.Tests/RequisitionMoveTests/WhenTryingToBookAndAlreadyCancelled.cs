namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenTryingToBookAndAlreadyCancelled
    {
        private ReqMove sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new ReqMove(1, 1, 1, 1, null, 100, null, "LINN", "STORES", "FREE");
            this.sut.Cancel(new DateTime(2024, 1, 1));
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.sut.OkToBook(TestTransDefs.LinnDeptToStock).Should().BeFalse();
        }
    }
}
