namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionFactoryTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingLdreqFrom : ContextBase
    {
        private StoresFunction ldreq;
        private Nominal nominal;
        private Department department;
        private Part part;
        private StorageLocation from;
        private RequisitionHeader result;
        private StoresTransactionDefinition transactionDefinition;

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
            this.EmployeeRepository.FindByIdAsync(33087).Returns(employee);
            this.StoresFunctionRepository.FindByIdAsync("LDREQ").Returns(this.ldreq);
            this.DepartmentRepository.FindByIdAsync(this.department.DepartmentCode).Returns(this.department);
            this.NominalRepository.FindByIdAsync(this.nominal.NominalCode).Returns(this.nominal);
            this.StorageLocationRepository.FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>())
                .Returns(this.from);
            this.PartRepository.FindByIdAsync(this.part.PartNumber).Returns(this.part);
            this.CreationStrategyResolver.Resolve(Arg.Is<RequisitionHeader>(h => h.StoresFunction.FunctionCode == "LDREQ")).Returns(
                new LdreqCreationStrategy(
                    this.AuthService,
                    this.ReqRepository,
                    this.RequisitionManager,
                    this.Logger));
            this.ReqRepository.FindByIdAsync(Arg.Any<int>()).Returns(
                new ReqWithReqNumber(
                    123,
                    employee,
                    this.ldreq,
                    "F",
                    null,
                    null,
                    this.department,
                    this.nominal));

            this.result = this.Sut.CreateRequisition(
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
                "LINN").Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.result.Document1Name.Should().Be("REQ");
        }

        // this should move to unit tests of the relevant strategy itself
        // [Test]
        // public void ShouldPickStock()
        // {
        //     this.ReqStoredProcedures.Received(1)
        //         .PickStock(
        //             this.part.PartNumber,
        //             Arg.Any<int>(),
        //             1,
        //             1,
        //             this.from.LocationId,
        //             null,
        //             "LINN",
        //             this.transactionDefinition.TransactionCode);
        // }
        //
        // [Test]
        // public void ShouldCreateNominalPostings()
        // {
        //     this.ReqStoredProcedures.Received(1).CreateNominals(
        //         Arg.Any<int>(), 1, 1, this.nominal.NominalCode, this.department.DepartmentCode);
        // }
    }
}
