namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using System.Threading.Tasks;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingRSN : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.Rsn.FunctionCode)
                .Returns(TestFunctionCodes.Rsn);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToRSN.TransactionCode)
                .Returns(TestTransDefs.StockToRSN);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 123).Returns(true);
            this.result = await this.Sut.Validate(
                33087,
                TestFunctionCodes.Rsn.FunctionCode,
                "F",
                1234,
                "R",
                null,
                null,
                new LineCandidate
                {
                    PartNumber = "PART",
                    Qty = 1,
                    TransactionDefinition = TestTransDefs.StockToRSN.TransactionCode,
                    Moves = new[] { new MoveSpecification { Qty = 1, ToPallet = 123 } }
                });
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
