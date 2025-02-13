namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class WhenCancellingOnlyNonBookedLine
    {
        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.req = new RequisitionHeader(
                new Employee(),
                new StoresFunction { FunctionCode = "F1" },
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                null,
                null,
                "Goodbye Reqs");
            var line1 = new RequisitionLine(this.req.ReqNumber, 1);
            line1.Book(new DateTime(2024,1,1));

            var unbookedLine = new RequisitionLine(this.req.ReqNumber, 2);

            this.req.AddLine(line1);
            this.req.AddLine(unbookedLine);

            // cancel unbooked line, leaving only the sole booked line
            this.req.CancelLine(2, "reason", new Employee());
        }

        [Test]
        public void ShouldMarkHeaderAsBooked()
        {
            // since all non-cancelled lines are booked
            this.req.DateBooked.HasValue.Should().BeTrue();
        }
    }
}
