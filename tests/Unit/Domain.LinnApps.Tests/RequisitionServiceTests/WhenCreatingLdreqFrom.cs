using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Linn.Common.Domain;
using Linn.Stores2.Domain.LinnApps.Accounts;
using Linn.Stores2.Domain.LinnApps.Parts;
using Linn.Stores2.Domain.LinnApps.Requisitions;
using Linn.Stores2.Domain.LinnApps.Stock;
using Linn.Stores2.TestData.Requisitions;
using NSubstitute;
using NUnit.Framework;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests;

public class WhenCreatingLdreqFrom : ContextBase
{
    private User user;
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
        
        this.user = new User { Privileges = new List<string> { "ldreq" }, UserNumber = 33087 };
        var employee = new Employee { Id = this.user.UserNumber };
        this.ldreq = new StoresFunction { FunctionCode = "LDREQ" };
        this.department = new Department { DepartmentCode = "DEPT" };
        this.nominal = new Nominal { NominalCode = "NOM" };
        this.from = new StorageLocation { LocationCode = "FROM", LocationId = 123};
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
            .Returns(new ProcessResult(true, null));
        
        this.ReqStoredProcedures.CreateNominals(
                Arg.Any<int>(), 1, 1, this.nominal.NominalCode, this.department.DepartmentCode)
            .Returns(new ProcessResult(true, null));

        this.ReqRepository.FindByIdAsync(Arg.Any<int>()).Returns(
            new ReqWithReqNumber(
                123,
                employee,
                this.ldreq,
                "F",
                123,
                "REQ",
                this.department, 
                this.nominal));
        
        this.result = this.Sut.CreateRequisition(this.user, this.ldreq.FunctionCode, "F", null, null,
            this.department.DepartmentCode, this.nominal.NominalCode,
            new LineCandidate
            {
                StockPicks = new List<MoveSpecification>
                {
                    new()
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
            "LINN").Result;
    }

    [Test]
    public void ShouldReturnCreated()
    {
        this.result.ReqNumber.Should().Be(123);
    }

    [Test]
    public void ShouldPickStock()
    {
        this.ReqStoredProcedures.Received(1)
            .PickStock(
                this.part.PartNumber,
                Arg.Any<int>(),
                1,
                1,
                this.from.LocationId,
                null,
                "LINN",
                this.transactionDefinition.TransactionCode);
    }

    [Test]
    public void ShouldCreateNominalPostings()
    {
        this.ReqStoredProcedures.Received(1).CreateNominals(
            Arg.Any<int>(), 1, 1, this.nominal.NominalCode, this.department.DepartmentCode);
    }
}
