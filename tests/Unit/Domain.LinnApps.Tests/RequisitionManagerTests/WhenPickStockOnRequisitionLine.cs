namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenPickStockOnRequisitionLine : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public void SetUp()
        {
            var employee = new Employee { Id = 33087 };

            var unpickedReq = new ReqWithReqNumber(
                123,
                employee,
                TestFunctionCodes.LoanOut,
                null,
                123,
                "L",
                null,
                null,
                fromStockPool: "LINN");
            var unpickedLine = new RequisitionLine(
                unpickedReq.ReqNumber,
                1,
                TestParts.SelektHub,
                1,
                TestTransDefs.StockToLoan);
            unpickedReq.AddLine(unpickedLine);

            var pickedReq = new ReqWithReqNumber(
                123,
                employee,
                TestFunctionCodes.LoanOut,
                null,
                123,
                "L",
                null,
                null,
                fromStockPool: "LINN");
            var pickedLine = new RequisitionLine(
                pickedReq.ReqNumber,
                1,
                TestParts.SelektHub,
                1,
                TestTransDefs.StockToLoan);
            pickedLine.Moves.Add(new ReqMove());
            pickedReq.AddLine(pickedLine);

            var lineWithPicks = new LineCandidate
            {
                LineNumber = 1,
                Document1Type = "L",
                Document1 = 123,
                Document1Line = 1,
                PartNumber = TestParts.SelektHub.PartNumber,
                Qty = 1,
                TransactionDefinition = TestTransDefs.StockToLoan.TransactionCode,
                Moves = new List<MoveSpecification>
                            {
                                new MoveSpecification { FromPallet = 512, Qty = 1 }
                            }
            };

            this.ReqRepository.FindByIdAsync(pickedReq.ReqNumber).Returns(pickedReq);

            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLoan.TransactionCode)
                .Returns(TestTransDefs.StockToLoan);

            this.ReqStoredProcedures
                .PickStock(
                    Arg.Any<string>(),
                    Arg.Any<int>(),
                    1,
                    1,
                    null,
                    512,
                    "LINN",
                    Arg.Any<string>())
                .Returns(new ProcessResult(
                    true, string.Empty));

            this.result = this.Sut.PickStockOnRequisitionLine(unpickedReq, lineWithPicks).Result;
        }

        [Test]
        public void ShouldPickStock()
        {
            this.ReqStoredProcedures.Received(1)
                .PickStock(
                    Arg.Any<string>(),
                    Arg.Any<int>(),
                    1,
                    1,
                    null,
                    512,
                    "LINN",
                    Arg.Any<string>());
        }

        [Test]
        public void ShouldReturnPickedReq()
        {
            this.result.ReqNumber.Should().Be(123);
            var line = this.result.Lines.Single(l => l.LineNumber == 1);
            line.Should().NotBeNull();
            line.Moves.Should().NotBeNull();
            line.Moves.Count.Should().Be(1);
        }
    }
}
