namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class WhenTryingToBookAndAlreadyBooked
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(
                123,
                1,
                new Part(),
                10,
                new StoresTransactionDefinition());

            this.sut.Book(new DateTime(2025, 2, 14));
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.sut.OkToBook().Should().BeFalse();
        }
    }
}
