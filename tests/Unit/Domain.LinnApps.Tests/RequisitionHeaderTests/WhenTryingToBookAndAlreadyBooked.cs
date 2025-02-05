namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using FluentAssertions;

    public class WhenTryingToBookAndAlreadyBooked
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionHeader(
                new Employee(),
                new StoresFunctionCode { FunctionCode = "F1" },
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                new List<RequisitionLine> { new LineWithMoves(123, 1) },
                null,
                "Goodbye Reqs");
            this.sut.Book(new Employee());
        }

        [Test]
        public void ShouldNotBeAbleToBook()
        {
            this.sut.CanBookReq(null).Should().BeFalse();
        }
    }
}
