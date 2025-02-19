namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenTryingToBook
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            var line = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.StockToLinnDept)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, null, null, null, null) },
            };
            line.AddPosting("D", 1, TestNominalAccounts.TestNomAcc);
            line.AddPosting("C", 1, TestNominalAccounts.AssetsRawMat);

            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "A Good Book");

            this.sut.AddLine(line);
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.sut.CanBookReq(null).Should().BeTrue();
        }
    }
}
