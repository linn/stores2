namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using FluentAssertions;

    public class WhenCheckingAuthorisation
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            var line = new RequisitionLine(123, 1, TestParts.SelektHub, 1, TestTransDefs.StockToLoan)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, 18414, "LN ON LOAN", "STORES", "FREE") },
            };
            line.AddPosting("D", 1, TestNominalAccounts.AssetsLoanGoods);
            line.AddPosting("C", 1, TestNominalAccounts.AssetsFinGoods);

            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LoanOut,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                new List<RequisitionLine> { line },
                null,
                "Can I steal the loan stock!");
        }

        [Test]
        public void ShouldRequireAuthorisation()
        {
            this.sut.RequiresAuthorisation().Should().BeTrue();
        }

        [Test]
        public void ShouldHaveAuthorisePrivilege()
        {
            this.sut.AuthorisePrivilege().Should().Be("stores.requisitions.AUTH");
        }
    }
}
