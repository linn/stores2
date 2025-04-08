namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingLoanOutAndLoanDoesNotExist : ContextBase
    {
        private Func<Task> act;

        [SetUp]
        public void SetUp()
        {
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.LoanOut.FunctionCode)
                .Returns(TestFunctionCodes.LoanOut);
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.LoanOut.FunctionCode,
                null,
                123,
                null,
                null,
                null,
                null);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("Loan Number 123 does not exist");
        }
    }
}
