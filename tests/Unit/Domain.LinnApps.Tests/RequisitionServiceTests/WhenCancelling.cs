namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancelling : ContextBase
    {
        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.req = new RequisitionHeader(
                123,
                "comment",
                new StoresFunctionCode { FunctionCode = "FUNC" },
                12345678,
                "TYPE");
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
                this.req.Document1Name).Returns(new ProcessResult(true, string.Empty));

            this.AuthService.HasPermissionFor(
                AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.req = this.Sut.CancelHeader(
                123, 
                user,
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
