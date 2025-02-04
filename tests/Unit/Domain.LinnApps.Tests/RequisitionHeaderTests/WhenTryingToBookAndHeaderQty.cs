namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using FluentAssertions;

    public class WhenTryingToBookAndHeaderQty
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            var line1 = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.SupplierToStores)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, null, null, null, null) },
            };
            line1.AddPosting("D", 1, TestNominalAccounts.AssetsRawMat);
            line1.AddPosting("C", 1, TestNominalAccounts.UninvoicedCreditors);

            var line2 = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.MaterialVarianceBelowStd)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, null, null, null, null) },
            };
            line2.AddPosting("D", 1, TestNominalAccounts.AssetsRawMat);
            line2.AddPosting("C", 1, TestNominalAccounts.UninvoicedCreditors);

            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.BookFromSupplier,
                "F",
                123456,
                "PO",
                null,
                null,
                new List<RequisitionLine> { line1 ,line2 },
                null,
                "Bought in bits",
                null, 
                null, 
                null, 
                null, 
                null, 
                null, 
                null,
                TestParts.Cap003,
                1
                );
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.sut.CanBookReq(null).Should().BeTrue();
        }
    }
}
