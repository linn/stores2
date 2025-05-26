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

    public class WhenValidatingBookLdReverseAndQtyWrong : ContextBase
    {
        private Func<Task> action;

        private IEnumerable<BookInOrderDetail> bookInOrderDetails;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.BookToLinnDepartment.FunctionCode)
                .Returns(TestFunctionCodes.BookToLinnDepartment);
            this.PartRepository.FindByIdAsync("PART")
                .Returns(new Part { PartNumber = "PART", BomVerifyFreqWeeks = 12 });
            this.bookInOrderDetails = new List<BookInOrderDetail>
                                          {
                                              new BookInOrderDetail
                                                  {
                                                      OrderNumber = 1234,
                                                      OrderLine = 1,
                                                      Sequence = 1,
                                                      Quantity = 1,
                                                      DepartmentCode = null,
                                                      NominalCode = null,
                                                      PartNumber = null,
                                                      ReqNumber = null,
                                                      IsReverse = "Y"
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
                partNumber: "PART",
                quantity: 1,
                isReverseTransaction: "Y",
                bookInOrderDetails: this.bookInOrderDetails);
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("You must specify a negative quantity for reverse but 1 supplied.");
        }
    }
}
