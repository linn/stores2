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
    using Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingLineAndUnallocatingFails : ContextBase
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
            var requisitionLine = new RequisitionLine(this.req.ReqNumber, 1);
            this.req.AddLine(requisitionLine);
            this.ReqRepository.FindByIdAsync(this.req.ReqNumber).Returns(this.req);

            var user = new User
                           {
                               UserNumber = 33087,
                               Privileges = new List<string>()
                           };
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee { Id = 33087 });

            this.ReqStoredProcedures.DeleteAllocOntos(
                this.req.ReqNumber,
                1,
                this.req.Document1.GetValueOrDefault(),
                this.req.Document1Name).Returns(new ProcessResult(false, "Some stores-y message"));

            this.AuthService.HasPermissionFor(
                AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.action = async () => await this.Sut.CancelLine(123, 1, user, "REASON");
        }

        [Test]
        public async Task ShouldThrowError()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Some stores-y message");
        }
    }
}
