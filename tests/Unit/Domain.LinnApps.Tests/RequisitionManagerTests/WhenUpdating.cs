namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private RequisitionHeader sut;

        [SetUp]
        public void Setup()
        {
            var dept = new Department { DepartmentCode = "0001" };
            var nom = new Nominal { NominalCode = "0002" };
            this.sut = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                12345678,
                "TYPE",
                dept,
                nom,
                null,
                null,
                "Goodbye Reqs");
            var requisitionLine = new RequisitionLine(
                123,
                1,
                new Part(),
                10,
                TestTransDefs.StockToLinnDept);
            this.sut.AddLine(requisitionLine);
            this.AuthService.HasPermissionFor(
                AuthorisedActions.GetRequisitionActionByFunction(
                    TestFunctionCodes.LinnDeptReq.FunctionCode),
                    Arg.Any<IEnumerable<string>>()).Returns(true);

            var part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(part.PartNumber).Returns(part);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(TestTransDefs.StockToLinnDept);
            this.NominalRepository.FindByIdAsync(nom.NominalCode).Returns(nom);

            var updatedDept = new Department { DepartmentCode = "0002" };
            this.DepartmentRepository.FindByIdAsync(updatedDept.DepartmentCode).Returns(updatedDept);

            this.StoresService.ValidDepartmentNominal(updatedDept.DepartmentCode, this.sut.Nominal.NominalCode)
                .Returns(new ProcessResult(true, string.Empty));

            this.Sut.UpdateRequisition(
                this.sut,
                "NEW COMMENT",
                "NEW REF",
                updatedDept.DepartmentCode,
                new List<LineCandidate>
                {
                },
                new List<string>());
        }

        [Test]
        public void ShouldUpdate()
        {
            this.sut.Comments.Should().Be("NEW COMMENT");
            this.sut.Reference.Should().Be("NEW REF");
            this.sut.Department.DepartmentCode.Should().Be("0002");
        }
    }
}
