﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

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
                TestFunctionCodes.LinnDeptReq,
                "F",
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
                TestTransDefs.StockToLinnDept);
            this.req.AddLine(requisitionLine);
            this.AuthService.HasPermissionFor(
                AuthorisedActions.GetRequisitionActionByFunction(
                    TestFunctionCodes.LinnDeptReq.FunctionCode),
                    Arg.Any<IEnumerable<string>>()).Returns(true);
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
                                                                    FromStockPool = "LINN",
                                                                    FromState = "STORES"
                                                                }
                                                        },
                                            TransactionDefinition = TestTransDefs.StockToLinnDept.TransactionCode
                                        };

            var part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(part.PartNumber).Returns(part);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(TestTransDefs.StockToLinnDept);
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
                TestTransDefs.StockToLinnDept.TransactionCode).Returns(new ProcessResult(true, string.Empty));

            this.ReqStoredProcedures.CreateNominals(
                this.req.ReqNumber,
                this.newLineCandidate.Qty,
                this.newLineCandidate.LineNumber,
                nom.NominalCode,
                dept.DepartmentCode).Returns(new ProcessResult(true, string.Empty));
            this.StockService.ValidStockLocation(null, 123, "PART", 10, "STORES", "LINN")
                .Returns(new ProcessResult(true, "Ok"));

            await this.Sut.UpdateRequisition(
                this.req,
                this.req.Comments,
                this.req.Reference,
                this.req.Department?.DepartmentCode,
                new List<LineCandidate>
                    {
                        this.newLineCandidate
                    },
                new List<string>());
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
                TestTransDefs.StockToLinnDept.TransactionCode);
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
