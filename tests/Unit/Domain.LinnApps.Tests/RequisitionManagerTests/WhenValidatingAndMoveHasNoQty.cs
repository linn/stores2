namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingAndMoveHasNoQty : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.DepartmentRepository.FindByIdAsync("1607")
                .Returns(new Department("1607", "DESC"));
            this.NominalRepository.FindByIdAsync("2963")
                .Returns(new Nominal("2963", "DESC"));
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.LinnDeptReq.FunctionCode)
                .Returns(TestFunctionCodes.LinnDeptReq);
            this.action = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.LinnDeptReq.FunctionCode,
                "F",
                null,
                null,
                "1607",
                "2963",
                new LineCandidate
                    {
                        Moves = new[] { new MoveSpecification { Qty = 0 } }
                    });
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<RequisitionException>().WithMessage("Move qty is invalid");
        }
    }
}
