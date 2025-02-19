namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionFactoryTests
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
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenCreatingLdreqFromAndPickStockFails : ContextBase
    {
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
            var employee = new Employee { Id = 33087 };

            this.ldreq = new StoresFunction { FunctionCode = "LDREQ" };

            this.department = new Department { DepartmentCode = "DEPT" };

            this.nominal = new Nominal { NominalCode = "NOM" };

            this.from = new StorageLocation { LocationCode = "FROM", LocationId = 123 };

            this.part = new Part { PartNumber = "PART" };

            this.transactionDefinition = new StoresTransactionDefinition { TransactionCode = "TRANS" };

            this.AuthService.HasPermissionFor(AuthorisedActions.Ldreq, Arg.Any<IEnumerable<string>>()).Returns(true);

            this.EmployeeRepository.FindByIdAsync(employee.Id).Returns(employee);

            this.StoresFunctionRepository.FindByIdAsync("LDREQ").Returns(this.ldreq);

            this.DepartmentRepository.FindByIdAsync(this.department.DepartmentCode).Returns(this.department);

            this.NominalRepository.FindByIdAsync(this.nominal.NominalCode).Returns(this.nominal);

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

            this.RequisitionManager.AddRequisitionLine(Arg.Any<RequisitionHeader>(), Arg.Any<LineCandidate>())
                .Throws(new PickStockException("failed in pick_stock - no stock found"));

            this.ReqRepository.FindByIdAsync(Arg.Any<int>()).Returns(this.createdReq);

            this.CreationStrategyResolver.Resolve(this.ldreq).Returns(
                new LdreqCreationStrategy(
                    this.AuthService,
                    this.ReqRepository,
                    this.RequisitionManager,
                    this.Logger));

            this.action = async () => await this.Sut.CreateRequisition(
                employee.Id,
                new List<string>(),
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
                    TransactionDefinition = this.transactionDefinition.TransactionCode
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
                    "Req failed to create since first line could not be added. Reason: failed in pick_stock - no stock found");
        }

        [Test]
        public void ShouldCancelHeader()
        {
            this.action();
            this.RequisitionManager.Received(1).CancelHeader(
                Arg.Any<int>(),
                33087,
                Arg.Any<IEnumerable<string>>(),
                 "Req failed to create since first line could not be added. Reason: failed in pick_stock - no stock found",
                false);
        }
    }
}
