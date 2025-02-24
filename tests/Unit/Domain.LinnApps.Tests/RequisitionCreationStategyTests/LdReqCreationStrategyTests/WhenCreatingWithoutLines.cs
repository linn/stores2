namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStategyTests.LdReqCreationStrategyTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingWithoutLines : ContextBase
    {
        private RequisitionHeader result;

        private RequisitionCreationContext context;

        [SetUp]
        public async Task SetUp()
        {
            this.context = new RequisitionCreationContext
            {
                UserPrivileges = new List<string>(),
                PartNumber = "PART",
                Quantity = 1,
                DepartmentCode = "0001234",
                ReqType = "F",
                NominalCode = "0004321",
                CreatedByUserNumber = 12345,
                Function = new StoresFunction("LDREQ")
                {
                    DepartmentNominalRequired = "Y",
                    TransactionsTypes = new List<StoresFunctionTransaction>
                                            {
                                                new StoresFunctionTransaction
                                                    {
                                                        ReqType = "F",
                                                        TransactionCode = "DEF"
                                                    }
                                            }
                }
            };
            var employee = new Employee();
            var dept = new Department();
            var nom = new Nominal();
            this.DepartmentRepository.FindByIdAsync(this.context.DepartmentCode).Returns(dept);
            this.NominalRepository.FindByIdAsync(this.context.NominalCode).Returns(nom);
            this.EmployeeRepository.FindByIdAsync(this.context.CreatedByUserNumber).Returns(employee);
            this.AuthService.HasPermissionFor(
                    AuthorisedActions.Ldreq,
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.Repository.FindByIdAsync(Arg.Any<int>())
                .Returns(
                    new RequisitionHeader(
                        employee,
                        this.context.Function,
                        "F",
                        null,
                        string.Empty,
                        dept,
                        nom));

            this.result = await this.Sut.Create(this.context);
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.result.Should().NotBe(null);
        }

        [Test]
        public void ShouldAdd()
        {
            this.Repository.Received(1).AddAsync(Arg.Any<RequisitionHeader>());
        }

        [Test]
        public void ShouldAddLine()
        {
            this.RequisitionManager.Received(1).AddRequisitionLine(
                Arg.Any<RequisitionHeader>(),
                this.context.FirstLineCandidate);
        }
    }
}
