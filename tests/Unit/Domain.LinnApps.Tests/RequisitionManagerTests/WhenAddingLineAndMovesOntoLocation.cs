﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingLineAndMovesOntoLocation : ContextBase
    {
        private RequisitionHeader header;

        private LineCandidate line;

        private Part part;

        private Nominal nominal;

        private Department department;

        [SetUp]
        public void SetUp()
        {
            this.nominal = new Nominal { NominalCode = "CODE" };
            this.department = new Department { DepartmentCode = "CODE" };

            this.DepartmentRepository.FindByIdAsync(this.department.DepartmentCode)
                .Returns(this.department);
            this.NominalRepository.FindByIdAsync(this.nominal.NominalCode).Returns(this.nominal);
            this.header = new RequisitionHeader(
                new Employee { Id = 33087 },
                TestFunctionCodes.LinnDeptReq,
                "O",
                null,
                null,
                this.department,
                this.nominal);
            this.part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(this.part.PartNumber).Returns(this.part);
            var loc = new StorageLocation(
                111,
                "E-L-1",
                "Desc",
                new StorageSite(),
                new StorageArea(),
                new AccountingCompany(),
                "Y",
                "Y",
                null,
                "N",
                "A",
                "A",
                null,
                null);
            this.StorageLocationRepository.FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>())
                .Returns(loc);
            this.line = new LineCandidate
            {
                LineNumber = 1,
                Document1 = 123,
                Document1Line = 1,
                Document1Type = "REQ",
                Moves = new List<MoveSpecification>
                                {
                                    new MoveSpecification
                                        {
                                            ToLocation = "E-L-1",
                                            Qty = 10,
                                            ToState = "STORES",
                                            ToStockPool = "LINN"
                                        }
                                },
                PartNumber = this.part.PartNumber,
                Qty = 10,
                TransactionDefinition = TestTransDefs.LinnDeptToStock.TransactionCode
            };
            this.DepartmentRepository.FindByIdAsync(this.department.DepartmentCode)
                .Returns(this.department);
            this.NominalRepository.FindByIdAsync(this.nominal.NominalCode)
                .Returns(this.nominal);
            this.ReqStoredProcedures
                .PickStock(
                    this.part.PartNumber,
                    Arg.Any<int>(),
                    1,
                    10,
                    111,
                    null,
                    "LINN",
                    TestTransDefs.LinnDeptToStock.TransactionCode)
                .Returns(new ProcessResult(
                    true, string.Empty));
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.LinnDeptToStock.TransactionCode)
                .Returns(TestTransDefs.LinnDeptToStock);
            this.ReqStoredProcedures.CreateNominals(
                Arg.Any<int>(),
                10,
                1,
                this.nominal.NominalCode,
                this.department.DepartmentCode).Returns(
                new ProcessResult(true, string.Empty));
            var state = new StockState("STORES", "DESC");
            this.StateRepository.FindByIdAsync("STORES").Returns(state);
            this.StoresService.ValidOntoLocation(this.part, loc, null, state)
                .Returns(new ProcessResult(true, string.Empty));
            this.ReqStoredProcedures.InsertReqOntos(
                Arg.Any<int>(),
                10,
                1,
                111,
                null,
                "LINN",
                "STORES",
                "FREE").Returns(new ProcessResult(true, string.Empty));

            this.Sut.AddRequisitionLine(this.header, this.line);
        }

        [Test]
        public void ShouldAdd()
        {
            this.header.Lines.Count.Should().Be(1);
        }

        [Test]
        public void ShouldCreateOntos()
        {
            this.ReqStoredProcedures.Received(1)
                .InsertReqOntos(
                    Arg.Any<int>(),
                    10, 
                    1,
                    111,
                    null, 
                    "LINN", 
                    "STORES",
                    "FREE");
        }

        [Test]
        public void ShouldCreateNominalPostings()
        {
            this.ReqStoredProcedures.Received(1).CreateNominals(
                Arg.Any<int>(), 10, 1, this.nominal.NominalCode, this.department.DepartmentCode);
        }
    }
}
