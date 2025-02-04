namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;
    using System;
    using FluentAssertions;

    public class WhenTryingToBookAndCancelled
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionLine(1, 1);

            this.sut.Cancel(100, "For Test", new DateTime(2025, 2, 14));
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.sut.OkToBook().Should().BeFalse();
        }
    }
}
