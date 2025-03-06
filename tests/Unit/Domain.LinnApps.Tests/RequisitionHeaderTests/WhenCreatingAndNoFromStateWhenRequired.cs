namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCreatingAndNoFromStateWhenRequired
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.AdjustQC,
                "F",
                null,
                null,
                new Department(),
                new Nominal("0000004710", "NOT STOCK ADJUSTMENTS"),
                reference: null,
                comments: "adjust qc test");
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .WithMessage("Cannot create - from state must be present");
        }
    }
}
