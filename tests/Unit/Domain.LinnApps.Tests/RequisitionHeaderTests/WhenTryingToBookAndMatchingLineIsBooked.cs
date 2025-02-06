namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using FluentAssertions;

    public class WhenTryingToBookAndMatchingLineIsBooked
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            var line = new LineWithMoves(123, 1);
            line.Book(new DateTime(2024,1,1));

            this.sut = new RequisitionHeader(
                new Employee(),
                new StoresFunction { FunctionCode = "F1" },
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                new List<RequisitionLine> { line },
                null,
                "Good book");
            this.sut.Book(new Employee());
        }

        [Test]
        public void ShouldNotBeAbleToBook()
        {
            this.sut.CanBookReq(null).Should().BeFalse();
        }
    }
}
