namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCheckingAndBookingHeader : ContextBase
    {
        private RequisitionHeader req;

        private decimal quantity;

        private int employeeId;

        [SetUp]
        public async Task SetUp()
        {
            this.employeeId = 567;
            this.quantity = 49;
            var line = new RequisitionLine(123, 1, TestParts.Cap003, this.quantity, TestTransDefs.StockToLinnDept)
                           {
                               Moves = { new ReqMove(123, 1, 1, this.quantity, 1, null, null, null, null, null) },
                           };
            line.AddPosting("D", this.quantity, TestNominalAccounts.TestNomAcc);
            line.AddPosting("C", this.quantity, TestNominalAccounts.AssetsRawMat);
            this.req = new ReqWithReqNumber(
                123,
                new Employee { Id = this.employeeId },
                TestFunctionCodes.LinnDeptReq,
                "F",
                null,
                "REQ",
                new Department(),
                new Nominal(),
                part: new Part { PartNumber = "P1" },
                quantity: this.quantity,
                fromState: "STORES",
                toState: "STORES");
            this.req.AddLine(line);
            this.ReqRepository.FindByIdAsync(Arg.Any<int>()).Returns(this.req);

            this.StoresService.ValidOntoLocation(
                Arg.Is<Part>(p => p.PartNumber == "P1"),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>())
                .Returns(new ProcessResult(true, "ok"));
            this.StoresService.ValidState(null, Arg.Is<StoresFunction>(a => a.FunctionCode == "FUNC"), "S1", "F")
                .Returns(new ProcessResult(true, "From State Ok"));
            this.StoresService.ValidState(null, Arg.Is<StoresFunction>(a => a.FunctionCode == "FUNC"), "S2", "O")
                .Returns(new ProcessResult(true, "From State Ok"));
            this.ReqStoredProcedures.CreateRequisitionLines(123, null)
                .Returns(new ProcessResult(true, "lines ok"));
            this.ReqStoredProcedures.DoRequisition(Arg.Any<int>(), null, this.employeeId)
                .Returns(new ProcessResult(true, "still ok"));
            this.StateRepository.FindByIdAsync("S2")
                .Returns(new StockState("S2", "S2 desc"));

            await this.Sut.CheckAndBookRequisition(this.req);
        }

        [Test]
        public void ShouldNotCreateLines()
        {
            this.ReqStoredProcedures.DidNotReceive().CreateRequisitionLines(123, null);
        }
        
        [Test]
        public void ShouldDoReq()
        {
            this.ReqStoredProcedures.Received().DoRequisition(123, null, this.employeeId);
        }

        [Test]
        public void ShouldCallDoRequisition()
        {
            this.ReqStoredProcedures.Received()
                .DoRequisition(
                    123,
                    null,
                    this.employeeId);
        }
    }
}
