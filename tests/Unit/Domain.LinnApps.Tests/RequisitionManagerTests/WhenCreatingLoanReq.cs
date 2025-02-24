namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.TestData.NominalAccounts;
    using System.Threading.Tasks;

    public class WhenCreatingLoanReq : ContextBase
    {
        private RequisitionHeader req;

        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            var line = new RequisitionLine(123, 1, TestParts.SelektHub, 1, TestTransDefs.StockToLinnDept)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, null, null, null, null) },
            };
            line.AddPosting("D", 1, TestNominalAccounts.AssetsFinGoods);
            line.AddPosting("C", 1, TestNominalAccounts.AssetsFinGoods);

            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LoanOut,
                "F",
                100,
                "L",
                new Department("0000002508", "ASSETS"),
                new Nominal("0000012087", "LOAN STOCK"));
            this.req.Lines.Add(line);

            this.ReqRepository.FindByIdAsync(123).Returns(this.req);

            var processResult = new ProcessResult(true, "123");

            this.ReqStoredProcedures.CreateLoanReq(100).Returns(processResult);

            this.result = await this.Sut.CreateLoanReq(100);
        }

        [Test]
        public void ShouldMakeReq()
        {
            this.result.Should().NotBeNull();
            this.result.ReqNumber.Should().Be(123);
        }
    }
}
