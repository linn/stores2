namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCreatingAndNoToLocationOrPalletWhenRequiredByStoresFunction
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => _ = new RequisitionHeader(
                                    new Employee(),
                                    TestFunctionCodes.SupplierKit,
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
                                    quantity: 10,
                                    toPalletNumber: null,
                                    fromPalletNumber: 123);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .WithMessage("To location or pallet required for: SUKIT");
        }
    }
}
