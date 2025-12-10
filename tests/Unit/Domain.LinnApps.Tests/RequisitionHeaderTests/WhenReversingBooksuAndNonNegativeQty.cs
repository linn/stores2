namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenReversingBooksuAndNonNegativeQty
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => _ = new RequisitionHeader(
                                    new Employee(),
                                    TestFunctionCodes.BookFromSupplier,
                                    "F",
                                    123,
                                    "PO",
                                    new Department(),
                                    new Nominal("0000004710", "NOT STOCK ADJUSTMENTS"),
                                    reference: null,
                                    comments: "revers reverse",
                                    isReversalOf: new RequisitionHeader(
                                        new Employee(),
                                        TestFunctionCodes.BookFromSupplier,
                                        "F",
                                        123,
                                        "PO",
                                        new Department(),
                                        new Nominal("0000004710", "NOT STOCK ADJUSTMENTS"),
                                        reference: null,
                                        comments: "original",
                                        quantity: 10,
                                        fromState: "QC",
                                        dateReceived: DateTime.UnixEpoch),
                                    quantity: 10,
                                    dateReceived: DateTime.UnixEpoch);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .Where(ex => ex.Message.Contains(
                    "Reversal quantity must be negative"));
        }
    }
}
