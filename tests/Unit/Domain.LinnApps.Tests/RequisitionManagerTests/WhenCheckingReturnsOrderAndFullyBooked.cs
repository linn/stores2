namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
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

    public class WhenCheckingReturnsOrderAndFullyBooked : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var emp = new Employee { Id = 1, Name = "Jock Bairn" };
            var part = new Part { PartNumber = "ADIKT", Description = "Cartridge" };
            var supplierLoc = new StorageLocation { LocationCode = "S-SU-1234", Description = "Supplier Heaven" };
            var header = new RequisitionHeader(
                emp,
                TestFunctionCodes.ReturnToSupplier,
                null,
                1,
                "RO",
                null,
                null,
                part: part,
                quantity: 1,
                toLocation: supplierLoc,
                document1Line: 1,
                fromState: "FAIL");

            var purchaseOrder = new PurchaseOrderResult
            {
                IsAuthorised = true,
                IsFilCancelled = false,
                OrderNumber = 1,
                DocumentType = "RO",
                Details = new List<PurchaseOrderDetailResult>
                {
                    new PurchaseOrderDetailResult { Line = 1, OurQty = 1, PartNumber = "ADIKT" }
                }
            };

            this.ReqStoredProcedures.GetQtyReturned(1, 1)
                .Returns(1);

            this.action = async () => await this.Sut.CheckReturnOrderForFullyBooked(header, purchaseOrder);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<DocumentException>().WithMessage("Returns Order 1/1 is fully booked");
        }
    }
}
