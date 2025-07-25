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

    public class WhenUpdatingAndInvalidDepartmentForNominal : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void Setup()
        {
            var dept = new Department { DepartmentCode = "0001" };
            var nom = new Nominal { NominalCode = "0002" };
            var req = new ReqWithReqNumber(
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
            req.AddLine(requisitionLine);
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

            this.StoresService.ValidDepartmentNominal(updatedDept.DepartmentCode, req.Nominal.NominalCode)
                .Returns(new ProcessResult(false, string.Empty));

            this.action = async () => await this.Sut.UpdateRequisition(
                req,
                req.Comments,
                req.Reference,
                updatedDept.DepartmentCode,
                new List<LineCandidate>
                {
                },
                new List<string>());
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>();
        }
    }
}
