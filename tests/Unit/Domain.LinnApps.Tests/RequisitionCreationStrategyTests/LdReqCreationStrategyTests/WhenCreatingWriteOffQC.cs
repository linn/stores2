namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LdReqCreationStrategyTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenCreatingWriteOffQC : ContextBase
    {
        private RequisitionHeader result;

        private RequisitionCreationContext context;

        [SetUp]
        public async Task SetUp()
        {
            this.context = new RequisitionCreationContext
            {
                UserPrivileges = new List<string>(),
                PartNumber = null,
                DepartmentCode = "0000042808",
                NominalCode = "0000004729",
                CreatedByUserNumber = 12345,
                FromState = "QC",
                ToState = null,
                Lines = new List<LineCandidate>
                            {
                                new LineCandidate
                                    {
                                        Qty = 1,
                                        LineNumber = 1,
                                        TransactionDefinition = TestTransDefs.WriteOffGoodsInInspection.TransactionCode,
                                        PartNumber = "PART",
                                    }
                            },
                Function = TestFunctionCodes.WriteOffFromQC
            };

            var employee = new Employee();
            var dept = new Department();
            var nom = new Nominal() { NominalCode = "0000004729" };
            this.DepartmentRepository.FindByIdAsync(this.context.DepartmentCode).Returns(dept);
            this.NominalRepository.FindByIdAsync(this.context.NominalCode).Returns(nom);
            this.EmployeeRepository.FindByIdAsync(this.context.CreatedByUserNumber).Returns(employee);
            this.AuthService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode),
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.StoresTransactionDefinitionRepository.FindByIdAsync(TestTransDefs.WriteOffGoodsInInspection.TransactionCode)
                .Returns(TestTransDefs.WriteOffGoodsInInspection);

            this.RequisitionManager.Validate(
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int?>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>()).ReturnsForAnyArgs(new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                null,
                null,
                new Department(),
                new Nominal()));

            this.Repository.FindByIdAsync(Arg.Any<int>())
                .Returns(
                    new RequisitionHeader(
                        employee,
                        TestFunctionCodes.WriteOffFromQC,
                        "F",
                        null,
                        string.Empty,
                        dept,
                        nom,
                        fromState: "QC"));

            this.result = await this.Sut.Create(this.context);
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.result.Should().NotBe(null);
        }
    }
}
