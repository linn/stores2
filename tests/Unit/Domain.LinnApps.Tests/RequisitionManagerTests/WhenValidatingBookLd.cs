namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
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

        [SetUp]
        public async Task SetUp()
        {
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.BookToLinnDepartment.FunctionCode)
                .Returns(TestFunctionCodes.BookToLinnDepartment);
            this.PartRepository.FindByIdAsync("PART").Returns(new Part { PartNumber = "PART", BomVerifyFreqWeeks = 12 });

            this.DocumentProxy.GetPurchaseOrder(123).Returns(
                new PurchaseOrderResult
                    {
                        OrderNumber = 123,
                        IsFilCancelled = false,
                        IsAuthorised = true,
                        DocumentType = "PO"
                    });
            
            this.result = await this.Sut.Validate(
                123,
                TestFunctionCodes.BookToLinnDepartment.FunctionCode,
                null,
                123,
                "PO",
                null,
                null,
                null,
                partNumber: "PART",
                quantity: 1);
        }

        [Test]
        public void ShouldGetOrder()
        {
            this.DocumentProxy.Received().GetPurchaseOrder(123);
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
