namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingBookLd : ContextBase
    {
        private RequisitionHeader result;

        private IEnumerable<BookInOrderDetail> bookInOrderDetails;

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.BookToLinnDepartment.FunctionCode)
                .Returns(TestFunctionCodes.BookToLinnDepartment);
            this.PartRepository.FindByIdAsync("SUNDRY PART")
                .Returns(new Part { PartNumber = "SUNDRY PART", BomVerifyFreqWeeks = 12 });
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
                                                      PartNumber = "SUNDRY PART"
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
                                                  PartNumber = "SUNDRY PART"
                                              }
                                      }
                    });
            
            this.result = await this.Sut.Validate(
                123,
                TestFunctionCodes.BookToLinnDepartment.FunctionCode,
                null,
                1234,
                "PO",
                null,
                null,
                null,
                partNumber: "SUNDRY PART",
                quantity: 1,
                bookInOrderDetails: this.bookInOrderDetails);
        }

        [Test]
        public void ShouldGetOrder()
        {
            this.DocumentProxy.Received().GetPurchaseOrder(1234);
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
