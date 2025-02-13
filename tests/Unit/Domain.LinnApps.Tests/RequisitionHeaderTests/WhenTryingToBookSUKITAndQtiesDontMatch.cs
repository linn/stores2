namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenTryingToBookSUKITAndQtiesDontMatch
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            var kittedAss = new Part { PartNumber = "MCP 2511/VP", Description = "KLIMAX DSM/3 - VIBRO POLISHED BUTTON CAP (MIDDLE)" };
            var kitpart1 = new Part { PartNumber = "MCP 2511", Description = "KLIMAX DSM/3 - VIBRO POLISHED BUTTON CAP (MIDDLE)" };
            var kitpart2 = new Part { PartNumber = "MCP 2512", Description = "KLIMAX DSM/3 - VIBRO POLISHED BUTTON CAP (END)" };

            var line1 = new RequisitionLine(123, 1, kitpart1, 400, TestTransDefs.StockToSupplierKit)
            {
                Moves = { new ReqMove(123, 1, 1, 400, 1, null, 18196, "SUPPLIER", "STORES", "FREE") },
            };
            line1.AddPosting("D", 400, TestNominalAccounts.AssetsRawMat);
            line1.AddPosting("C", 400, TestNominalAccounts.AssetsRawMat);

            var line2 = new RequisitionLine(123, 1, kitpart2, 200, TestTransDefs.StockToSupplierKit)
            {
                Moves = { new ReqMove(123, 1, 1, 200, 1, null, 18196, "SUPPLIER", "STORES", "FREE") },
            };
            line2.AddPosting("D", 200, TestNominalAccounts.AssetsRawMat);
            line2.AddPosting("C", 200, TestNominalAccounts.AssetsRawMat);

            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.SupplierKit,
                "F",
                123456,
                "PO",
                null,
                null,
                new List<RequisitionLine> { line1, line2 },
                null,
                "Kitting parts",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                kittedAss,
                200);
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.sut.CanBookReq(null).Should().BeTrue();
        }
    }
}
