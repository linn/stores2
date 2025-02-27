namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingLineAndMovesOnto : ContextBase
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
                new StoresFunction("LDREQ"),
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
                Moves = new List<MoveSpecification>
                                {
                                    new MoveSpecification
                                        {
                                            ToPallet = 512,
                                            Qty = 10,
                                            ToState = "STORES",
                                            ToStockPool = "LINN"
                                        }
                                },
                PartNumber = this.part.PartNumber,
                Qty = 10,
                TransactionDefinition = "DEF"
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
                    "LINN",
                    "DEF")
                .Returns(new ProcessResult(
                    true, string.Empty));
            this.TransactionDefinitionRepository.FindByIdAsync("DEF")
                .Returns(new StoresTransactionDefinition("DEF"));
            this.ReqStoredProcedures.CreateNominals(
                Arg.Any<int>(),
                10,
                1,
                this.nominal.NominalCode,
                this.department.DepartmentCode).Returns(
                new ProcessResult(true, string.Empty));
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 512).Returns(true);

            this.ReqStoredProcedures.InsertReqOntos(
                Arg.Any<int>(),
                10,
                1,
                null,
                512,
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
                    null,
                    512, 
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
