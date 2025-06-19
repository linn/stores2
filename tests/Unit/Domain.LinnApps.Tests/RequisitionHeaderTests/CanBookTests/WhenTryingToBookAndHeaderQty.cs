namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests.CanBookTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenTryingToBookAndHeaderQty : CanBookContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var line1 = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.SupplierToStores)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, 123, "LINN", "OK", "FREE") }
            };
            line1.AddPosting("D", 1, TestNominalAccounts.AssetsRawMat);
            line1.AddPosting("C", 1, TestNominalAccounts.UninvoicedCreditors);

            var line2 = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.MaterialVarianceBelowStd)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, 123, "LINN", "OK", "FREE") }
            };
            line2.AddPosting("D", 1, TestNominalAccounts.AssetsRawMat);
            line2.AddPosting("C", 1, TestNominalAccounts.UninvoicedCreditors);

            this.Sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.BookFromSupplier,
                null,
                123456,
                "PO",
                null,
                null,
                reference: null,
                comments: "Bought in bits",
                manualPick: null, 
                fromStockPool: null, 
                toStockPool: null, 
                fromPalletNumber: null, 
                toPalletNumber: null, 
                fromLocation: null, 
                toLocation: null,
                part: TestParts.Cap003,
                quantity: 1,
                dateReceived: DateTime.Today);
            this.Sut.AddLine(line1);
            this.Sut.AddLine(line2);

            this.Result = this.Sut.RequisitionCanBeBooked();
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.Result.Success.Should().BeTrue();
        }
    }
}
