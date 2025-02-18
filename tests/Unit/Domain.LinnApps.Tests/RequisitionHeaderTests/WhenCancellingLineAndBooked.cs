namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NUnit.Framework;

    public class WhenCancellingLineAndBooked
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var req = new RequisitionHeader(
                new Employee(),
                new StoresFunction { FunctionCode = "F1" },
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "Goodbye Reqs");
            req.AddLine(new RequisitionLine(null, 1));
            req.BookLine(1, new Employee(), new DateTime(2024,1,1));
            this.action = () => req.CancelLine(1, "reason", new Employee());
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>()
                .WithMessage("Cannot cancel a booked req line");
        }
    }
}
