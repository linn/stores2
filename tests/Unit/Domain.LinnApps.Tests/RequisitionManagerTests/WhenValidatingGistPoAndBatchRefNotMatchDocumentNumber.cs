namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingGistPoAndBatchRefNotMatchDocumentNumber : ContextBase
    {
        private Func<Task> act;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.GistPo.FunctionCode)
                .Returns(TestFunctionCodes.GistPo);
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 666).Returns(true);
            this.PartRepository.FindByIdAsync("PART").Returns(new Part { PartNumber = "PART" });
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.InspectionToStores.TransactionCode)
                .Returns(TestTransDefs.InspectionToStores);
            this.PalletRepository.FindByIdAsync(666).Returns(new StoresPallet { DateInvalid = null });
            this.DocumentProxy.GetPurchaseOrder(1234567).Returns(
                new PurchaseOrderResult
                {
                    IsAuthorised = true,
                    IsFilCancelled = false,
                    OrderNumber = 1234568,
                    DocumentType = "PO"
                });
            this.StateRepository.FindByIdAsync("STORES").Returns(new StockState("STORES", "Stores"));
            this.StockPoolRepository.FindByIdAsync("STORES").Returns(new StockPool());
            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, null));
            this.StockService.ValidStockLocation(null, 666, "PART", 10, "QC")
                .Returns(new ProcessResult(true, null));
            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.GistPo.FunctionCode,
                null,
                1234567,
                "PO",
                null,
                null,
                fromStockPool: "QC",
                toStockPool: "STORES",
                fromPalletNumber: 666,
                toPalletNumber: 666,
                partNumber: "PART",
                quantity: 10,
                fromState: "QC",
                toState: "STORES",
                batchRef: "P1234567");
        }


        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("You are trying to pass stock for payment from a different PO");
        }
    }
}
