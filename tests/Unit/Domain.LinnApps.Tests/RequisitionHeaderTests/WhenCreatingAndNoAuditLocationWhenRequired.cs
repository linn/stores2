﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCreatingAndNoAuditLocationWhenRequired
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => _ = new RequisitionHeader(
                                    new Employee(),
                                    TestFunctionCodes.Audit,
                                    null,
                                    null,
                                    null,
                                    new Department(),
                                    new Nominal("0000004710", "NOT STOCK ADJUSTMENTS"),
                                    reference: null,
                                    comments: null);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .WithMessage($"You must specify an audit location for {TestFunctionCodes.Audit.FunctionCode}.");
        }
    }
}
