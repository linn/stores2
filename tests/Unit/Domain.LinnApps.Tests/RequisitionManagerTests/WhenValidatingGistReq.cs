using Linn.Common.Domain;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;
    
    public class WhenValidatingGistReq : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.GistReq.FunctionCode)
                .Returns(TestFunctionCodes.GistReq);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.InspectionToStores2.TransactionCode)
                .Returns(TestTransDefs.InspectionToStores2);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 123).Returns(true);
            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(), 
                Arg.Any<StorageLocation>(), 
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, string.Empty));
            this.result = await this.Sut.Validate(
                33087,
                TestFunctionCodes.GistReq.FunctionCode,
                null,
                null,
                null,
                null,
                null,
                fromStockPool: "LINN",
                toPalletNumber: 123,
                partNumber: "PART",
                quantity: 1,
                fromState: "QC",
                toState: "STORES");
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    } 
}

