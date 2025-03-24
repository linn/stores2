namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingReqAndAddingLine : ContextBase
    {
        private RequisitionHeader req;

        private LineCandidate newLineCandidate;

        [SetUp]
        public async Task Setup()
        {
            var dept = new Department { DepartmentCode = "0001" };
            var nom = new Nominal { NominalCode = "0002" };
            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                new StoresFunction { FunctionCode = "F1" },
                null,
                12345678,
                "TYPE",
                dept,
                nom,
                null,
                null,
                "Goodbye Reqs");
            var requisitionLine = new RequisitionLine(
                123,
                1,
                new Part(),
                10,
                new StoresTransactionDefinition());
            this.req.AddLine(requisitionLine);

            this.newLineCandidate = new LineCandidate
                                        {
                                            PartNumber = "PART",
                                            LineNumber = 2,
                                            Qty = 10,
                                            Moves = new List<MoveSpecification>
                                                        {
                                                            new MoveSpecification
                                                                {
                                                                    FromPallet = 123,
                                                                    Qty = 10,
                                                                    FromStockPool = "LINN"
                                                                }
                                                        },
                                            TransactionDefinition = "DEF"
                                        };

            var part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(part.PartNumber).Returns(part);
            this.TransactionDefinitionRepository.FindByIdAsync("DEF")
                .Returns(new StoresTransactionDefinition("DEF"));
            this.DepartmentRepository.FindByIdAsync(dept.DepartmentCode).Returns(dept);
            this.NominalRepository.FindByIdAsync(nom.NominalCode).Returns(nom);

            this.ReqStoredProcedures.PickStock(
                "PART",
                this.req.ReqNumber,
                this.newLineCandidate.LineNumber,
                this.newLineCandidate.Qty,
                null,
                123,
                "LINN",
                "DEF").Returns(new ProcessResult(true, string.Empty));

            this.ReqStoredProcedures.CreateNominals(
                this.req.ReqNumber,
                this.newLineCandidate.Qty,
                this.newLineCandidate.LineNumber,
                nom.NominalCode,
                dept.DepartmentCode).Returns(new ProcessResult(true, string.Empty));

            await this.Sut.UpdateRequisition(
                this.req,
                this.req.Comments,
                new List<LineCandidate>
                    {
                        this.newLineCandidate
                    });
        }

        [Test]
        public void ShouldPickStock()
        {
            this.ReqStoredProcedures.Received(1).PickStock(
                "PART",
                this.req.ReqNumber,
                this.newLineCandidate.LineNumber,
                this.newLineCandidate.Qty,
                null,
                123,
                "LINN",
                "DEF");
        }

        [Test]
        public void ShouldAddLine()
        {
            this.req.Lines.Count.Should().Be(2);
            this.req.Lines.Last().Part.PartNumber.Should().Be("PART");
            this.req.Lines.Last().Qty.Should().Be(10);
        }
    }
}
