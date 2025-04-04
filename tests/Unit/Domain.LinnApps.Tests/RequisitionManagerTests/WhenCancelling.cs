namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancelling : ContextBase
    {
        private RequisitionHeader req;

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
                this.req.Document1Name).Returns(new ProcessResult(true, string.Empty));

            this.AuthService.HasPermissionFor(
                AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.req = this.Sut.CancelHeader(
                this.req.ReqNumber, 
                employee.Id,
                new List<string>(),
                "REASON").Result;
        }

        [Test]
        public void ShouldUnallocateStock()
        {
            this.ReqStoredProcedures.Received()
                .DeleteAllocOntos(
                    this.req.ReqNumber,
                    null,
                    this.req.Document1.GetValueOrDefault(),
                    this.req.Document1Name);
        }

        [Test]
        public void ShouldReturnCancelled()
        {
            this.req.ReqNumber.Should().Be(123);
            this.req.Cancelled.Should().Be("Y");
            this.req.CancelDetails.Count.Should().Be(1);
        }
    }
}
