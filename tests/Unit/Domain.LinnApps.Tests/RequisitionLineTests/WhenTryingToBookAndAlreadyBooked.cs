namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;
    using System;
    using FluentAssertions;
    using Linn.Stores2.TestData.Transactions;

    public class WhenTryingToBookAndAlreadyBooked
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(1, 1);

            this.sut.Book(new DateTime(2025, 2, 14));
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.sut.OkToBook().Should().BeFalse();
        }
    }
}
