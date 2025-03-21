namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingLine : ContextBase
    {
        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                null,
                null,
                "Goodbye Reqs");
            var requisitionLine = new RequisitionLine(123, 1, new Part(), 10, TestTransDefs.StockToLinnDept);
            this.req.AddLine(requisitionLine);
            this.ReqRepository.FindByIdAsync(this.req.ReqNumber).Returns(this.req);

            var employee = new Employee { Id = 33087 };

            this.EmployeeRepository.FindByIdAsync(33087).Returns(employee);

            this.ReqStoredProcedures.DeleteAllocOntos(
                this.req.ReqNumber,
                1,
                this.req.Document1.GetValueOrDefault(),
                this.req.Document1Name).Returns(new ProcessResult(true, string.Empty));

            this.AuthService.HasPermissionFor(
                AuthorisedActions.CancelRequisition, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.req = this.Sut.CancelLine(
                123, 
                1,
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
                    1,
                    this.req.Document1.GetValueOrDefault(),
                    this.req.Document1Name);
        }

        [Test]
        public void ShouldReturnCancelled()
        {
            this.req.Cancelled.Should().Be("Y");
            this.req.CancelDetails.Count.Should().Be(1);
        }
    }
}
