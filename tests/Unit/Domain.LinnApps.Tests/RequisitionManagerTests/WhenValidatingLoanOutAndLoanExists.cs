namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingLoanOutAndLoanExists : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public void SetUp()
        {
            this.DocumentProxy.GetLoan(123)
                .Returns(new LoanResult { LoanNumber = 123 });
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.LoanOut.FunctionCode)
                .Returns(TestFunctionCodes.LoanOut);
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.result = this.Sut.Validate(
                33087, 
                TestFunctionCodes.LoanOut.FunctionCode,
                null,
                123,
                null,
                null,
                null).Result;
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
