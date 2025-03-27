namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCreatingAndToStateNotAllowedForFunction
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () =>
                {
                    _ = new RequisitionHeader(
                        new Employee(),
                        TestFunctionCodes.GistPo,
                        "F",
                        1234567,
                        null,
                        new Department(),
                        new Nominal("0000004710", "STOCK ADJUSTMENTS"),
                        reference: null,
                        comments: "constructor test",
                        fromState: "QC", 
                        toState: "QC",
                        quantity: 10,
                        fromStockPool: "LINN",
                        toStockPool: "LINN");
                };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .WithMessage(
                    "Validation failed with the following errors: To state must be one of STORES");
        }
    }
}
