namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.TestData.FunctionCodes;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingLoanBackAndSpecifiedLineNumberDoesNotExist : ContextBase
    {
        private Func<Task> act;

        [SetUp]
        public void SetUp()
        {
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.LoanBack.FunctionCode)
                .Returns(TestFunctionCodes.LoanBack);
            this.DocumentProxy.GetLoan(123)
                .Returns(
                    new LoanResult
                    {
                        LoanNumber = 123,
                        Details = new List<LoanDetail>
                        {
                            new LoanDetail
                            {
                                LineNumber = 1, ArticleNumber = "ART", IsCancelled = true, Quantity = 10
                            }
                        }
                    });
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.LoanBack.FunctionCode,
                null,
                123,
                "L",
                null,
                null,
                document1Line: 6,
                lines: null);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage("Loan number 123 does not have a line number 6");
        }
    }
}
