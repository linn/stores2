﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenCancellingLineAndBooked
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => new BookedRequisitionHeader(
                    123, 33087, new StoresFunctionCode { FunctionCode = "C" })
                .CancelLine(1, "reason", new Employee());
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>()
                .WithMessage("Cannot cancel a booked req line");
        }
    }
}
