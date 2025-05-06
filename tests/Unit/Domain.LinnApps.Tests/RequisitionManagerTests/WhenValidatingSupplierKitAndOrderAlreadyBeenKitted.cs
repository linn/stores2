namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
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

    public class WhenValidatingSupplierKitAndOrderAlreadyBeenKitted : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.SupplierKit.FunctionCode)
                .Returns(TestFunctionCodes.SupplierKit);
            this.ReqStoredProcedures.CanPutPartOnPallet("ADIKT", 666).Returns(true);
            var part = new Part { PartNumber = "ADIKT" };
            this.PartRepository.FindByIdAsync("ADIKT").Returns(part);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.InspectionToStores.TransactionCode)
                .Returns(TestTransDefs.InspectionToStores);
            var loc = new StorageLocation { LocationId = 1, LocationCode = "S-SU-1234" };
            this.StorageLocationRepository.FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>())
                .Returns(loc);
            this.DocumentProxy.GetPurchaseOrder(827753).Returns(
                new PurchaseOrderResult
                {
                    IsAuthorised = true,
                    IsFilCancelled = false,
                    OrderNumber = 827753,
                    DocumentType = "PO",
                    Details = new List<PurchaseOrderDetailResult>
                    {
                        new PurchaseOrderDetailResult { Line = 1, OurQty = 1, PartNumber = "ADIKT" }
                    }
                });
            this.StateRepository.FindByIdAsync("STORES").Returns(new StockState("STORES", "Stores"));
            this.StockPoolRepository.FindByIdAsync("SUPPLIER").Returns(new StockPool());
            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, null));
            this.StockService.ValidStockLocation(null, 666, "PART", 10, "QC")
                .Returns(new ProcessResult(true, null));

            var requisitions = new List<RequisitionHeader>
            {
                new RequisitionHeader(
                    new Employee(),
                    TestFunctionCodes.SupplierKit,
                    null,
                    1,
                    "PO",
                    null,
                    null,
                    part: part,
                    quantity: 6,
                    toLocation: loc),
                new RequisitionHeader(
                    new Employee(),
                    TestFunctionCodes.SupplierKit,
                    null,
                    1,
                    "PO",
                    null,
                    null,
                    part: part,
                    quantity: 3,
                    toLocation: loc)
            };
            this.ReqRepository.FilterByAsync(Arg.Any<Expression<Func<RequisitionHeader, bool>>>())
                .Returns(requisitions);

            this.action = () => this.Sut.Validate(
                                    33087,
                                    TestFunctionCodes.SupplierKit.FunctionCode,
                                    null,
                                    827753,
                                    "PO",
                                    null,
                                    null,
                                    null,
                                    partNumber: "ADIKT",
                                    quantity: 1,
                                    fromState: "STORES",
                                    toState: "STORES",
                                    toLocationCode: "S-SU-1234",
                                    toStockPool: "SUPPLIER");
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<DocumentException>()
                .WithMessage("Full order qty 1 on order 827753 has already been kitted");
        }
    }
}
