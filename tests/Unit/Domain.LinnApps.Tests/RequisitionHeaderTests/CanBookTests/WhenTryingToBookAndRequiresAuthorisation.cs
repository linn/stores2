namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests.CanBookTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenTryingToBookAndRequiresAuthorisation : CanBookContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var line1 = new RequisitionLine(123, 1, TestParts.SelektHub, 1, TestTransDefs.StockToLoan)
                            {
                                Moves = { new ReqMove(123, 1, 1, 1, 1, null, 123, "LINN", "OK", "FREE") }
                            };
            line1.AddPosting("D", 1, TestNominalAccounts.AssetsRawMat);
            line1.AddPosting("C", 1, TestNominalAccounts.UninvoicedCreditors);

            this.Sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LoanOut,
                null,
                12345678,
                "L",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "Don't steal the loan stock!");
            this.Sut.AddLine(line1);

            this.Result = this.Sut.RequisitionCanBeBooked();
        }

        [Test]
        public void ShouldNotBeAbleToBook()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("Req 0 requires authorisation.");
        }
    }
}
