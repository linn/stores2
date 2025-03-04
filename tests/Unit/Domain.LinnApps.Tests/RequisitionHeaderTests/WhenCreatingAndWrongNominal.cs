namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using NUnit.Framework;
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;

    public class WhenCreatingAndWrongNominal
    {
        private Action action;


        [SetUp]
        public void SetUp()
        {
            this.action = () => new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.AdjustReq,
                "F",
                null,
                null,
                new Department(),
                new Nominal("0000001234", "NOT STOCK ADJUSTMENTS"),
                reference: null,
                comments: "constructor test");
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .WithMessage("Cannot create - nominal must be 0000004710");
        }
    }
}
