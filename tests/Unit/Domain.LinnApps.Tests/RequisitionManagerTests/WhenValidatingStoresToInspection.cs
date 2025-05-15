namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingStoresToInspection : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.StoresToInspection.FunctionCode)
                .Returns(TestFunctionCodes.StoresToInspection);
            this.ReqStoredProcedures.CanPutPartOnPallet(TestParts.Cap003.PartNumber, 503).Returns(true);
            this.PartRepository.FindByIdAsync(TestParts.Cap003.PartNumber).Returns(TestParts.Cap003);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StoresToInspection.TransactionCode)
                .Returns(TestTransDefs.StoresToInspection);
            this.PalletRepository.FindByIdAsync(503).Returns(new StoresPallet());
            this.StateRepository.FindByIdAsync("STORES").Returns(new StockState("STORES", "Stores"));
            this.StateRepository.FindByIdAsync("FAIL").Returns(new StockState("FAIL", "Fail"));
            this.StockPoolRepository.FindByIdAsync("LINN").Returns(new StockPool());
            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, null));
            this.StockService.ValidStockLocation(null, 500, TestParts.Cap003.PartNumber, 1, "STORES").Returns(new ProcessResult(true, null));
            this.result = await this.Sut.Validate(
                33087,
                TestFunctionCodes.StoresToInspection.FunctionCode,
                null,
                null,
                null,
                null,
                null,
                null,
                partNumber: TestParts.Cap003.PartNumber,
                quantity: 1,
                fromState: "STORES",
                toState: "FAIL",
                fromPalletNumber: 500,
                toPalletNumber: 503,
                fromStockPool: "LINN",
                toStockPool: "LINN");
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
