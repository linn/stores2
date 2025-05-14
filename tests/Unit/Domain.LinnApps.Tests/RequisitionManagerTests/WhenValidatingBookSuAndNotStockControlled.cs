namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingBookSuAndNotStockControlled : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.BookFromSupplier.FunctionCode)
                .Returns(TestFunctionCodes.BookFromSupplier);
            this.PartRepository.FindByIdAsync("PART")
                .Returns(new Part { PartNumber = "PART", StockControlled = "N" });

            this.DocumentProxy.GetPurchaseOrder(1234).Returns(
                new PurchaseOrderResult
                    {
                        OrderNumber = 1234,
                        IsFilCancelled = false,
                        IsAuthorised = true,
                        DocumentType = "PO",
                        Details = new List<PurchaseOrderDetailResult>
                                      {
                                          new PurchaseOrderDetailResult
                                              {
                                                  PartNumber = "PART"
                                              }
                                      }
                    });
            this.StoresService.ValidOntoLocation(
                Arg.Any<Part>(),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, null));

            this.action = () => this.Sut.Validate(
                                    123,
                                    TestFunctionCodes.BookFromSupplier.FunctionCode,
                                    null,
                                    1234,
                                    "PO",
                                    null,
                                    null,
                                    null,
                                    partNumber: "PART",
                                    quantity: 1,
                                    toState: "STORES",
                                    dateReceived: null);
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("BOOKSU requires part to be stock controlled and PART is not.");
        }
    }
}
