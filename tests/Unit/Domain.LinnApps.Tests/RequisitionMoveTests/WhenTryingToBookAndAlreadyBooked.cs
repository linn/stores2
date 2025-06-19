namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenTryingToBookAndAlreadyBooked : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Book(new DateTime(2025, 2, 14));
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.Sut.MoveIsBookable(TestTransDefs.LinnDeptToStock).Should().BeFalse();
        }
    }
}
