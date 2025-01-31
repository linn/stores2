using Linn.Stores2.Domain.LinnApps.Accounts;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenCancellingAndBooked
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => new RequisitionHeader(
                123, 
                "comm", 
                new StoresFunctionCode { FunctionCode = "C" },
                123,
                "REQ",
                new Department(),
                new Nominal(),
                null)
                .Cancel("reason", new Employee());
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>()
                .WithMessage("Cannot cancel a booked req");
        }
    }
}
