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
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingBookLdAndQuantityNotMatch : ContextBase
    {
        private Func<Task> action;

        private IEnumerable<BookInOrderDetail> bookInOrderDetails;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.BookToLinnDepartment.FunctionCode)
                .Returns(TestFunctionCodes.BookToLinnDepartment);
            this.PartRepository.FindByIdAsync("SUNDRY PART")
                .Returns(new Part { PartNumber = "SUNDRY PART", StockControlled = "N" });
            this.bookInOrderDetails = new List<BookInOrderDetail>
                                          {
                                              new BookInOrderDetail
                                                  {
                                                      OrderNumber = 1234,
                                                      OrderLine = 1,
                                                      Sequence = 1,
                                                      Quantity = 2,
                                                      DepartmentCode = "0000011111",
                                                      NominalCode = "0000022222",
                                                      PartNumber = "SUNDRY PART",
                                                      ReqNumber = null,
                                                      IsReverse = "N"
                                                  }
                                          };
            this.DocumentProxy.GetPurchaseOrder(1234).Returns(
                new PurchaseOrderResult
                    {
                        OrderNumber = 1234,
                        IsFilCancelled = false,
                        IsAuthorised = true,
                        DocumentType = "PO"
                    });
            
            this.action = () => this.Sut.Validate(
                123,
                TestFunctionCodes.BookToLinnDepartment.FunctionCode,
                null,
                1234,
                "PO",
                null,
                null,
                partNumber: "SUNDRY PART",
                quantity: 4,
                bookInOrderDetails: this.bookInOrderDetails);
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("Book in order detail quantity (2) does not match req quantity (4).");
        }
    }
}
