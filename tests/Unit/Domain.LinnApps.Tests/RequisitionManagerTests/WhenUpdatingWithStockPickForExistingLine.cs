namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingWithStockPickForExistingLine : ContextBase
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
                "F",
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
                TestTransDefs.StockToLinnDept);
            this.req.AddLine(requisitionLine);

            this.line = new LineCandidate
            {
                PartNumber = "PART",
                LineNumber = 1,
                Qty = 10,
                StockPicked = true,
                Moves = new List<MoveSpecification>
                            {
                                new MoveSpecification
                                    {
                                        FromPallet = 123,
                                        Qty = 10,
                                        FromStockPool = "LINN"
                                    }
                            },
                TransactionDefinition = TestTransDefs.StockToLinnDept.TransactionCode
            };

            this.AuthService.HasPermissionFor(
                AuthorisedActions.GetRequisitionActionByFunction(TestFunctionCodes.LinnDeptReq.FunctionCode),
                Arg.Any<IEnumerable<string>>()).Returns(true);

            var part = new Part { PartNumber = "PART" };
            this.PartRepository.FindByIdAsync(part.PartNumber).Returns(part);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(TestTransDefs.StockToLinnDept);
            this.DepartmentRepository.FindByIdAsync(dept.DepartmentCode).Returns(dept);
            this.NominalRepository.FindByIdAsync(nom.NominalCode).Returns(nom);

            this.ReqStoredProcedures.PickStock(
                "PART",
                this.req.ReqNumber,
                this.line.LineNumber,
                this.line.Qty,
                null,
                123,
                "LINN",
                TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(new ProcessResult(true, string.Empty));

            this.ReqStoredProcedures.CreateNominals(
                this.req.ReqNumber,
                this.line.Qty,
                this.line.LineNumber,
                nom.NominalCode,
                dept.DepartmentCode).Returns(new ProcessResult(true, string.Empty));

            await this.Sut.UpdateRequisition(
                this.req,
                this.req.Comments,
                this.req.Reference,
                this.req.Department?.DepartmentCode,
                new List<LineCandidate>
                    {
                        this.line
                    },
                new List<string>());
        }

        [Test]
        public void ShouldPickStock()
        {
            this.ReqStoredProcedures.Received(1).PickStock(
                "PART",
                this.req.ReqNumber,
                this.line.LineNumber,
                this.line.Qty,
                null,
                123,
                "LINN",
                TestTransDefs.StockToLinnDept.TransactionCode);
        }
    }
}
