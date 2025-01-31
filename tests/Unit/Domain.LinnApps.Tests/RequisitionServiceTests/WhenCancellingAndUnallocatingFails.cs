namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingAndUnallocatingFails : ContextBase
    {
        private RequisitionHeader req;
        
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.req = new RequisitionHeader(
                123,
                "comment",
                new StoresFunctionCode { FunctionCode = "FUNC" },
                12345678,
                "TYPE", new Department(), new Nominal(), null);
            this.ReqRepository.FindByIdAsync(this.req.ReqNumber).Returns(this.req);

            var user = new User
                           {
                               UserNumber = 33087,
                               Privileges = new List<string>()
                           };
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee { Id = 33087 });

            this.ReqStoredProcedures.DeleteAllocOntos(
                this.req.ReqNumber,
                null,
                this.req.Document1.GetValueOrDefault(),
                this.req.Document1Name).Returns(new ProcessResult(false, "Some stores-y message"));

            this.AuthService.HasPermissionFor(
                AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.action = async () => await this.Sut.CancelHeader(123, user, "REASON");
        }

        [Test]
        public async Task ShouldThrowError()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Some stores-y message");
        }
    }
}
