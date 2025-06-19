namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenTryingToBookAndAlreadyBooked : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Book(new DateTime(2025, 2, 14));

            this.result = this.Sut.LineIsBookable();
        }

        [Test]
        public void ShouldNotBeOkToBook()
        {
            this.result.Should().BeFalse();
        }
    }
}
