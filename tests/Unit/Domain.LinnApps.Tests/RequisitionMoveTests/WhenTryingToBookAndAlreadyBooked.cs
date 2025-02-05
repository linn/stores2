namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;
    using System;
    using FluentAssertions;
    using Linn.Stores2.TestData.Transactions;

    public class WhenTryingToBookAndAlreadyBooked
    {
        private ReqMove sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new ReqMove(1, 1, 1, 1, null, 100, null, "LINN", "STORES", "FREE");
            this.sut.Book(new DateTime(2025,2,14));
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.sut.OkToBook(TestTransDefs.LinnDeptToStock).Should().BeFalse();
        }
    }
}
