namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenTryingToBookAndAlreadyCancelled : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Cancel(new DateTime(2024, 1, 1));
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.Sut.MoveIsBookable(TestTransDefs.LinnDeptToStock).Should().BeFalse();
        }
    }
}
