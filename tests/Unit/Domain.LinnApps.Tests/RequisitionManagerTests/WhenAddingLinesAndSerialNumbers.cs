namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenAddingLinesAndSerialNumbers : ContextBase
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
                "F",
                null,
                null,
                this.department,
                this.nominal);
            this.part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(this.part.PartNumber).Returns(this.part);
            this.line = new LineCandidate
            {
                LineNumber = 1,
                Document1 = 123,
                Document1Line = 1,
                Document1Type = "REQ",
                PartNumber = this.part.PartNumber,
                Qty = 1,
                Moves = new List<MoveSpecification>
                                                 {
                                                     new MoveSpecification
                                                         {
                                                             FromPallet = 512,
                                                             Qty = 1,
                                                             FromStockPool = "LINN"
                                                         }
                                                 },
                TransactionDefinition = TestTransDefs.StockToLinnDept.TransactionCode,
                SerialNumbers = new List<int>() { 1234 }
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
                    1,
                    null,
                    512,
                    "LINN",
                    TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(new ProcessResult(
                    true, string.Empty));
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(new StoresTransactionDefinition(TestTransDefs.StockToLinnDept.TransactionCode));
            this.ReqStoredProcedures.CreateNominals(
                Arg.Any<int>(),
                1,
                1,
                this.nominal.NominalCode,
                this.department.DepartmentCode).Returns(
                new ProcessResult(true, string.Empty));

            this.Sut.AddRequisitionLine(this.header, this.line);
        }

        [Test]
        public void ShouldAdd()
        {
            this.header.Lines.Count.Should().Be(1);
        }

        [Test]
        public void ShouldHaveSerialNumbers()
        {
            this.header.Lines.First().SerialNumbers.Should().NotBeNull();
            this.header.Lines.First().SerialNumbers.Count().Should().Be(1);
        }

        [Test]
        public void ShouldCommit()
        {
            this.TransactionManager.Received(1).CommitAsync();
        }
    }
}
