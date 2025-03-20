namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCreatingAndNoFromLocationOrPalletWhenRequiredByStoresFunction
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => _ = new RequisitionHeader(
                                    new Employee(),
                                    TestFunctionCodes.AdjustLoc,
                                    "F",
                                    123,
                                    null,
                                    new Department(),
                                    new Nominal("0000004710", "NOT STOCK ADJUSTMENTS"),
                                    reference: null,
                                    comments: "adjust qc test",
                                    toStockPool: "POOL",
                                    fromState: "STATE",
                                    fromStockPool: "A",
                                    quantity: 10);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .WithMessage(
                    "Validation failed with the following errors: From location or pallet required for: ADJUST LOC");
        }
    }
}
