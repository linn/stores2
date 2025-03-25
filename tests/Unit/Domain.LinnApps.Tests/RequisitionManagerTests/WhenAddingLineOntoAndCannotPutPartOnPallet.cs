namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
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
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingLineOntoAndCannotPutPartOnPallet : ContextBase
    {
        private RequisitionHeader header;

        private LineCandidate line;

        private Part part;

        private Nominal nominal;

        private Department department;

        private Func<Task> action;

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
                "F",
                null,
                null,
                this.department,
                this.nominal,
                toStockPool: "LINN",
                toState: "STATE");
            this.part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(this.part.PartNumber).Returns(this.part);
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
                                            ToPallet = 512,
                                            Qty = 10
                                        }
                                },
                PartNumber = this.part.PartNumber,
                Qty = 10,
                TransactionDefinition = TestTransDefs.StockToLinnDept.TransactionCode
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
                    null,
                    512,
                    this.header.ToStockPool,
                    TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(new ProcessResult(
                    true, string.Empty));
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(new StoresTransactionDefinition(TestTransDefs.StockToLinnDept.TransactionCode));
            this.ReqStoredProcedures.CreateNominals(
                Arg.Any<int>(),
                10,
                1,
                this.nominal.NominalCode,
                this.department.DepartmentCode).Returns(
                new ProcessResult(true, string.Empty));
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 512).Returns(false);

            this.ReqStoredProcedures.InsertReqOntos(
                Arg.Any<int>(),
                10,
                1,
                null,
                512,
                this.header.ToStockPool,
                this.header.ToState,
                "FREE").Returns(new ProcessResult(true, string.Empty));
            this.PalletRepository.FindByIdAsync(512)
                .Returns(new StoresPallet());
            this.action = () => this.Sut.AddRequisitionLine(this.header, this.line);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<CannotPutPartOnPalletException>()
                .WithMessage("Cannot put part PART onto P512");
        }
    }
}
