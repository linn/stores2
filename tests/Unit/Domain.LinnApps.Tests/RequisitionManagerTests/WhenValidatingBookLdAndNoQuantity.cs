namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingBookLdAndNoQuantity : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
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
            
            this.action = () => this.Sut.Validate(
                123,
                TestFunctionCodes.BookToLinnDepartment.FunctionCode,
                null,
                123,
                "PO",
                null,
                null,
                null,
                partNumber: "PART",
                quantity: 0);
        }

        [Test]
        public async Task ShouldThrowCorrectException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("You must specify a quantity to book for PO 123.");
        }
    }
}
