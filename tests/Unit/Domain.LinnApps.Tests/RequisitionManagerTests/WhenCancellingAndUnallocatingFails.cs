namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingAndUnallocatingFails : ContextBase
    {
        private RequisitionHeader req;
        
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                new StoresFunction { FunctionCode = "FUNC" },
                "F",
                123,
                "REQ",
                new Department(),
                new Nominal());
            this.ReqRepository.FindByIdAsync(this.req.ReqNumber).Returns(this.req);
            var employee = new Employee { Id = 33087 };
            this.EmployeeRepository.FindByIdAsync(33087).Returns(employee);

            this.ReqStoredProcedures.DeleteAllocOntos(
                this.req.ReqNumber,
                null,
                this.req.Document1.GetValueOrDefault(),
                this.req.Document1Name).Returns(new ProcessResult(false, "Some stores-y message"));

            this.AuthService.HasPermissionFor(
                AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.action = async () => await this.Sut.CancelHeader(123, employee.Id, new List<string>(), "REASON");
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<CancelRequisitionException>()
                .WithMessage("Some stores-y message");
        }
    }
}
