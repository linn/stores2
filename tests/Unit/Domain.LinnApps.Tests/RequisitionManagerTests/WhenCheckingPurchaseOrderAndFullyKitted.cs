namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenCheckingPurchaseOrderAndFullyKitted : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var emp = new Employee { Id = 1, Name = "Jock Bairn" };
            var part = new Part { PartNumber = "ADIKT", Description = "Cartridge" };
            var supplierLoc = new StorageLocation { LocationCode = "S-SU-1234", Description = "Supplier Heaven" };
            var requisitions = new List<RequisitionHeader>
                                   {
                                       new RequisitionHeader(
                                           emp,
                                           TestFunctionCodes.SupplierKit,
                                           null,
                                           1,
                                           "PO",
                                           null,
                                           null,
                                           part: part,
                                           quantity: 6,
                                           toLocation: supplierLoc),
                                       new RequisitionHeader(
                                           emp,
                                           TestFunctionCodes.SupplierKit,
                                           null,
                                           1,
                                           "PO",
                                           null,
                                           null,
                                           part: part,
                                           quantity: 3,
                                           toLocation: supplierLoc),
                                   };
            var header = new RequisitionHeader(
                emp,
                TestFunctionCodes.SupplierKit,
                null,
                1,
                "PO",
                null,
                null,
                part: part,
                quantity: 1,
                toLocation: supplierLoc);

            var purchaseOrder = new PurchaseOrderResult
            {
                IsAuthorised = true,
                IsFilCancelled = false,
                OrderNumber = 1,
                DocumentType = "PO",
                Details = new List<PurchaseOrderDetailResult>
                {
                    new PurchaseOrderDetailResult { Line = 1, OurQty = 9, PartNumber = "ADIKT" }
                }
            };

            this.ReqRepository.FilterByAsync(Arg.Any<Expression<Func<RequisitionHeader, bool>>>())
                .Returns(requisitions);

            this.action = async () => await this.Sut.CheckPurchaseOrderForOverAndFullyKitted(header, purchaseOrder);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<DocumentException>().WithMessage($"Full order qty 9 on order 1 has already been kitted");
        }
    }
}
