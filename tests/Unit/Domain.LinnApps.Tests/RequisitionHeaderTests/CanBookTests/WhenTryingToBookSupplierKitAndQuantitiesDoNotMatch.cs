namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests.CanBookTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenTryingToBookSupplierKitAndQuantitiesDoNotMatch : CanBookContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var kittedAss = new Part
                                {
                                    PartNumber = "MCP 2511/VP",
                                    Description = "KLIMAX DSM/3 - VIBRO POLISHED BUTTON CAP (MIDDLE)"
                                };
            var kitPart1 = new Part
                               {
                                   PartNumber = "MCP 2511",
                                   Description = "KLIMAX DSM/3 - VIBRO POLISHED BUTTON CAP (MIDDLE)"
                               };
            var kitPart2 = new Part
                               {
                                   PartNumber = "MCP 2512",
                                   Description = "KLIMAX DSM/3 - VIBRO POLISHED BUTTON CAP (END)"
                               };

            var line1 = new RequisitionLine(123, 1, kitPart1, 400, TestTransDefs.StockToSupplierKit)
                            {
                                Moves = { new ReqMove(123, 1, 1, 400, 1, null, 18196, "SUPPLIER", "STORES", "FREE") },
                            };

            line1.AddPosting("D", 400, TestNominalAccounts.AssetsRawMat);
            line1.AddPosting("C", 400, TestNominalAccounts.AssetsRawMat);

            var line2 = new RequisitionLine(123, 1, kitPart2, 200, TestTransDefs.StockToSupplierKit)
                            {
                                Moves = { new ReqMove(123, 1, 1, 200, 1, null, 18196, "SUPPLIER", "STORES", "FREE") },
                            };

            line2.AddPosting("D", 200, TestNominalAccounts.AssetsRawMat);
            line2.AddPosting("C", 200, TestNominalAccounts.AssetsRawMat);

            this.Sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.SupplierKit,
                null,
                123456,
                "PO",
                null,
                null,
                reference: null,
                comments: "Kitting parts",
                manualPick: null,
                fromStockPool: null,
                toStockPool: null,
                fromPalletNumber: null,
                toPalletNumber: 123,
                fromLocation: null,
                toLocation: null,
                part: kittedAss,
                quantity: 200);
            this.Sut.AddLine(line1);
            this.Sut.AddLine(line2);

            this.Result = this.Sut.RequisitionCanBeBooked();
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.Result.Success.Should().BeTrue();
            this.Result.Message.Should().Be("Function code does not require line quantity check.");
        }
    }
}
