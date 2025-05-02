namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class WhenValidatingReturnToSupplierAndPurchaseOrder : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.ReturnToSupplier.FunctionCode)
                .Returns(TestFunctionCodes.ReturnToSupplier);
            this.ReqStoredProcedures.CanPutPartOnPallet("ADIKT", 666).Returns(true);
            this.PartRepository.FindByIdAsync("ADIKT").Returns(new Part { PartNumber = "ADIKT" });
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.InspectionToStores.TransactionCode)
                .Returns(TestTransDefs.InspectionToStores);
            this.DocumentProxy.GetPurchaseOrder(894006).Returns(
                new PurchaseOrderResult
                {
                    IsAuthorised = true,
                    IsFilCancelled = false,
                    OrderNumber = 894006,
                    DocumentType = "PO",
                    Details = new List<PurchaseOrderDetailResult>
                    {
                        new PurchaseOrderDetailResult { Line = 1, OurQty = 1, PartNumber = "ADIKT" }
                    }
                });
            this.StateRepository.FindByIdAsync("STORES").Returns(new StockState("STORES", "Stores"));
            this.StockPoolRepository.FindByIdAsync("SUPPLIER").Returns(new StockPool());

            this.ReqStoredProcedures.GetQtyReturned(894006, 1)
                .Returns(0);

            var loc = new StorageLocation { LocationId = 1, LocationCode = "E-PUR-RET" };
            this.StorageLocationRepository.FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>())
                .Returns(loc);

            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, null));

            this.StockService.ValidStockLocation(1, null, "ADIKT", 1, "STORES").Returns(new ProcessResult(true, null));

            this.action = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.ReturnToSupplier.FunctionCode,
                null,
                894006,
                "PO",
                null,
                null,
                null,
                partNumber: "ADIKT",
                quantity: 1,
                fromState: "STORES",
                fromLocationCode: "E-PUR-RET",
                document1Line: 1);
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("Order 894006 is not a returns/credit order!");
        }
    }
}
