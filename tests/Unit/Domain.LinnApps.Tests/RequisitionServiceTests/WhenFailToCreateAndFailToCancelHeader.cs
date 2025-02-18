namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenFailToCreateAndFailToCancelHeader : ContextBase
    {
        private User user;
        private StoresFunction ldreq;
        private Nominal nominal;
        private Department department;
        private Part part;
        private StorageLocation from;
        private StoresTransactionDefinition transactionDefinition;

        private Func<Task> action;

        private RequisitionHeader createdReq;

        [SetUp]
        public void Setup()
        {
            this.user = new User { Privileges = new List<string> { "ldreq" }, UserNumber = 33087 };
            var employee = new Employee { Id = this.user.UserNumber };

            this.ldreq = new StoresFunction { FunctionCode = "LDREQ" };

            this.department = new Department { DepartmentCode = "DEPT" };

            this.nominal = new Nominal { NominalCode = "NOM" };

            this.from = new StorageLocation { LocationCode = "FROM", LocationId = 123 };

            this.part = new Part { PartNumber = "PART" };

            this.transactionDefinition = new StoresTransactionDefinition { TransactionCode = "TRANS" };

            this.AuthService.HasPermissionFor(AuthorisedActions.Ldreq, this.user.Privileges).Returns(true);

            this.EmployeeRepository.FindByIdAsync(this.user.UserNumber).Returns(employee);

            this.StoresFunctionRepository.FindByIdAsync("LDREQ").Returns(this.ldreq);

            this.DepartmentRepository.FindByIdAsync(this.department.DepartmentCode).Returns(this.department);

            this.NominalRepository.FindByIdAsync(this.nominal.NominalCode).Returns(this.nominal);

            this.TransactionDefinitionRepository.FindByIdAsync("TRANS").Returns(this.transactionDefinition);

            this.StorageLocationRepository.FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>())
                .Returns(this.from);

            this.PartRepository.FindByIdAsync(this.part.PartNumber).Returns(this.part);

            this.createdReq = new ReqWithReqNumber(
                123,
                employee,
                this.ldreq,
                "F",
                123,
                "REQ",
                this.department,
                this.nominal);

            this.ReqStoredProcedures
                .PickStock(
                    this.part.PartNumber,
                    Arg.Any<int>(),
                    1,
                    1,
                    this.from.LocationId,
                    null,
                    "LINN",
                    this.transactionDefinition.TransactionCode)
                .Returns(new ProcessResult(true, string.Empty));

            this.ReqStoredProcedures.CreateNominals(
                    Arg.Any<int>(), 1, 1, this.nominal.NominalCode, this.department.DepartmentCode)
                .Returns(new ProcessResult(false, "can't post there"));

            this.ReqStoredProcedures.DeleteAllocOntos(Arg.Any<int>(), null, Arg.Any<int>(), this.createdReq.Document1Name)
                .Returns(new ProcessResult(false, "couldn't un-allocate stock!"));

            this.ReqRepository.FindByIdAsync(Arg.Any<int>()).Returns(this.createdReq);

            this.action = async () => await this.Sut.CreateRequisition(
                this.user,
                this.ldreq.FunctionCode,
                "F",
                null,
                null,
                this.department.DepartmentCode,
                this.nominal.NominalCode,
                new LineCandidate
                {
                    StockPicks = new List<MoveSpecification>
                    {
                    new ()
                    {
                        PartNumber = this.part.PartNumber, Qty = 1, FromLocation = this.from.LocationCode
                    }
                    },
                    LineNumber = 1,
                    PartNumber = this.part.PartNumber,
                    Qty = 1,
                    TransactionDefinition = transactionDefinition.TransactionCode
                },
                "ref",
                "comments",
                null,
                "LINN");
        }

        [Test]
        public async Task ShouldThrowError()
        {
            await this.action.Should()
                .ThrowAsync<CreateRequisitionException>()
                .WithMessage(
                    "Warning - req failed to create: failed in create_nominals: can't post there. "
                    + "Header also failed to cancel: couldn't un-allocate stock!. Some cleanup may be required!");
        }
    }
}
