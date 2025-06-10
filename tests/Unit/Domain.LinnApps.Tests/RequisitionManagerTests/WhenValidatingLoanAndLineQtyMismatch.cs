namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValidatingLoanAndLineQtyMismatch : ContextBase
    {
        private Func<Task> act;

        [SetUp]
        public void SetUp()
        {
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.LoanOut.FunctionCode)
                .Returns(TestFunctionCodes.LoanOut);
            this.DocumentProxy.GetLoan(123)
                .Returns(
                    new LoanResult
                    {
                        LoanNumber = 123, 
                        IsCancelled = false, 
                        Details = new List<LoanDetail>
                        {
                            new LoanDetail
                            {
                                LineNumber = 1, ArticleNumber = "ART", IsCancelled = false, Quantity = 10
                            }
                        }
                    });
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.act = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.LoanOut.FunctionCode,
                null,
                123,
                null,
                null,
                null,
                lines: new List<LineCandidate>
                {
                    new LineCandidate
                    {
                        PartNumber = "ART",
                        Qty = 100,
                        Document1Line = 1
                    }
                });
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.act.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Loan line 1 is for a qty of 10");
        }
    }
}
