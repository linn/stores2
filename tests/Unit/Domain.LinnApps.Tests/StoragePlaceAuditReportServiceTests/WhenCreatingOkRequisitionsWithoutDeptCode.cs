namespace Linn.Stores2.Domain.LinnApps.Tests.StoragePlaceAuditReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingOkRequisitionsWithoutDeptCode : ContextBase
    {
        private ProcessResult result;

        private int employeeNumber;

        private string departmentCode;

        private string employeeDepartmentCode;

        [SetUp]
        public async Task SetUp()
        {
            this.employeeNumber = 111;
            this.departmentCode = null;
            this.employeeDepartmentCode = "0000045678";

            this.DepartmentRepository.FindByIdAsync(this.employeeDepartmentCode)
                .Returns(new Department(this.employeeDepartmentCode, "Employee Department"));
            this.EmployeeRepository.FindByIdAsync(this.employeeNumber)
                .Returns(new Employee { Id = this.employeeNumber, DepartmentCode = this.employeeDepartmentCode });
            
            this.StoragePlaceQueryRepository.FilterBy(Arg.Any<Expression<Func<StoragePlace, bool>>>())
                .Returns(new List<StoragePlace> { new StoragePlace { PalletNumber = 745, Name = "P745" } }.AsQueryable());
            this.RequisitionFactory.CreateRequisition(
                this.employeeNumber,
                Arg.Any<List<string>>(),
                "AUDIT",
                null,
                null,
                null,
                null,
                null,
                null,
                this.employeeDepartmentCode,
                "0000004710",
                comments: "Correct",
                auditLocation: "P745",
                fromLocationCode: null,
                toLocationCode: null,
                fromPalletNumber: 745,
                toPalletNumber: 745,
                lines: Arg.Any<List<LineCandidate>>()).Returns(
                new ReqWithReqNumber(
                    1234,
                    new Employee { Id = this.employeeNumber },
                    TestFunctionCodes.Audit,
                    null,
                    null,
                    null,
                    new Department(this.departmentCode, "d"),
                    new Nominal("0000004710", "n"),
                    auditLocation: "P745"));

            this.result = await this.Sut.CreateSuccessAuditReqs(
                this.employeeNumber,
                new List<string> { "P745" },
                null,
                this.departmentCode,
                new List<string>());
        }

        [Test]
        public void ShouldCreateReq()
        {
            this.RequisitionFactory.Received().CreateRequisition(
                this.employeeNumber,
                Arg.Any<List<string>>(),
                "AUDIT",
                null,
                null,
                null,
                null,
                null,
                null,
                this.employeeDepartmentCode,
                "0000004710",
                comments: "Correct",
                auditLocation: "P745",
                fromLocationCode: null,
                toLocationCode: null,
                fromPalletNumber: 745,
                toPalletNumber: 745,
                lines: Arg.Any<List<LineCandidate>>());
        }

        [Test]
        public void ShouldBookReq()
        {
            this.RequisitionManager.Received().CheckAndBookRequisition(
                Arg.Is<RequisitionHeader>(a =>
                    a.StoresFunction.FunctionCode == "AUDIT" && a.CreatedBy.Id == this.employeeNumber));
        }

        [Test]
        public void ShouldReturnProcessResult()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
