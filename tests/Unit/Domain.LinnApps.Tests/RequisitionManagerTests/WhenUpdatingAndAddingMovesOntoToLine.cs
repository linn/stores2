namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndAddingMovesOntoToLine : ContextBase
    {
        private RequisitionHeader req;

        private LineCandidate line;

        [SetUp]
        public async Task Setup()
        {
            var dept = new Department { DepartmentCode = "0001" };
            var nom = new Nominal { NominalCode = "0002" };
            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "O",
                12345678,
                "TYPE",
                dept,
                nom,
                null,
                null,
                "Goodbye Reqs",
                null,
                "LINN");
            var requisitionLine = new RequisitionLine(
                123,
                1,
                new Part { PartNumber = "PART" },
                10,
                TestTransDefs.LinnDeptToStock);
            this.req.AddLine(requisitionLine);

            this.line = new LineCandidate
            {
                PartNumber = "PART",
                LineNumber = 1,
                Qty = 10,
                Moves = new List<MoveSpecification>
                            {
                                new MoveSpecification
                                    {
                                        ToPallet = null,
                                        ToLocation = "LOC",
                                        Qty = 10,
                                        ToStockPool = "LINN",
                                        ToState = "S",
                                        IsAddition = true
                                    }
                            },
                TransactionDefinition = TestTransDefs.LinnDeptToStock.TransactionCode
            };

            var part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(part.PartNumber).Returns(part);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.LinnDeptToStock.TransactionCode)
                .Returns(TestTransDefs.LinnDeptToStock);

            this.StorageLocationRepository.FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>())
                .Returns(new StorageLocation { LocationCode = "LOC", LocationId = 567 });

            this.ReqStoredProcedures.InsertReqOntos(
                                        this.req.ReqNumber,
                                        10,
                                        1,
                                        567,
                                        null,
                                        "LINN",
                                       "S",
                                        "FREE").Returns(new ProcessResult(true, null));

            await this.Sut.UpdateRequisition(
                this.req,
                this.req.Comments,
                new List<LineCandidate>
                    {
                        this.line
                    });
        }

        [Test]
        public void ShouldCreateMoves()
        {
            this.ReqStoredProcedures.Received(1).InsertReqOntos(
                this.req.ReqNumber,
                this.line.Qty,
                this.line.LineNumber,
                567,
                null,
                "LINN",
                "S",
                "FREE",
                "I");
        }
    }
}
