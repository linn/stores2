namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingCustRet : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.CustomerReturn.FunctionCode)
                .Returns(TestFunctionCodes.CustomerReturn);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.CustomerToGoodStock.TransactionCode)
                .Returns(TestTransDefs.CustomerToGoodStock);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 123).Returns(true);
            this.DocumentProxy.GetCreditNote(1234, null).Returns(new DocumentResult("C", 1234, null, null, null));
            this.result = await this.Sut.Validate(
                33087,
                TestFunctionCodes.CustomerReturn.FunctionCode,
                null,
                1234,
                "C",
                null,
                null,
                new LineCandidate
                {
                    PartNumber = "PART",
                    Qty = 1,
                    TransactionDefinition = TestTransDefs.CustomerToGoodStock.TransactionCode,
                    Moves = new[] { new MoveSpecification { Qty = 1, ToPallet = 123 } }
                },
                toStockPool: "LINN",
                toState: "STORES");
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
