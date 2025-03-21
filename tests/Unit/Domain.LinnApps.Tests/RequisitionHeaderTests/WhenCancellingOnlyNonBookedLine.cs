namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenCancellingOnlyNonBookedLine
    {
        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.req = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "Goodbye Reqs");
            var line1 = new RequisitionLine(123, 1, new Part(), 10, TestTransDefs.StockToLinnDept);
            line1.Book(new DateTime(2024, 1, 1));

            var unbookedLine = new RequisitionLine(123, 2, new Part(), 10, TestTransDefs.StockToLinnDept);

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
