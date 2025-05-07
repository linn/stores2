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
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingBookLdAndStockControlled : ContextBase
    {
        private Func<Task> action;

        private IEnumerable<BookInOrderDetail> bookInOrderDetails;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.BookToLinnDepartment.FunctionCode)
                .Returns(TestFunctionCodes.BookToLinnDepartment);
            this.PartRepository.FindByIdAsync("SC PART")
                .Returns(new Part { PartNumber = "SC PART", StockControlled = "Y" });
            this.bookInOrderDetails = new List<BookInOrderDetail>
                                          {
                                              new BookInOrderDetail
                                                  {
                                                      OrderNumber = 1243,
                                                      OrderLine = 1,
                                                      Sequence = 1,
                                                      Quantity = 1,
                                                      DepartmentCode = "0000011111",
                                                      NominalCode = "0000022222",
                                                      PartNumber = "SC PART"
                                                  }
                                          };
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
                                                  PartNumber = "SC PART"
                                              }
                                      }
                });

            this.action = () => this.Sut.Validate(
                123,
                TestFunctionCodes.BookToLinnDepartment.FunctionCode,
                null,
                1234,
                "PO",
                null,
                null,
                null,
                partNumber: "SC PART",
                quantity: 1,
                bookInOrderDetails: this.bookInOrderDetails);
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("Part SC PART is stock controlled and BOOKLD must be sundry.");
        }
    }
}
