namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LoanOutCreationStrategyTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        [SetUp]
        public async Task SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
            {
                Function = new StoresFunction("LOAN OUT"),
                Document1Type = "L",
                Document1Number = 100
            };


            var line = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.StockToLinnDept)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, null, null, null, null) }
            };
            line.AddPosting("D", 1, TestNominalAccounts.AssetsFinGoods);
            line.AddPosting("C", 1, TestNominalAccounts.AssetsFinGoods);

            var req = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LoanOut,
                "F",
                100,
                "L",
                new Department("0000002508", "ASSETS"),
                new Nominal("0000012087", "LOAN STOCK"),
                reference: null,
                comments: "I was made");
            req.AddLine(line);

            this.RequisitionManager.CreateLoanReq(100).Returns(req);

            this.Result = await this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public void ShouldCallManager()
        {
            this.RequisitionManager
                .Received()
                .CreateLoanReq(100);
        }

        [Test]
        public void ShoudReturnReq()
        {
            this.Result.Should().NotBeNull();
            this.Result.StoresFunction.Should().NotBeNull();
            this.Result.StoresFunction.FunctionCode.Should().Be("LOAN OUT");
        }

        [Test]
        public void ShouldDefaultExtraFields()
        {
            this.Result.ToCategory.Should().Be("FREE");
            this.Result.FromState.Should().Be("STORES");
            this.Result.ToState.Should().Be("STORES");
        }
    }
}
