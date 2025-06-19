namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAndBookingHeaderStockPoolInvalid : ContextBase
    {
        private decimal quantity;

        private int employeeId;

        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.employeeId = 567;
            this.quantity = 49;
            this.StateRepository.FindByIdAsync("S2")
                .Returns(new StockState("S2", "S2 desc"));
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.Move.FunctionCode)
                .Returns(TestFunctionCodes.Move);
            this.StoresService.ValidOntoLocation(
                Arg.Is<Part>(p => p.PartNumber == "P1"),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>())
                .Returns(new ProcessResult(true, "ok"));
            this.PartRepository.FindByIdAsync("P1").Returns(new Part { PartNumber = "P1" });
            this.PartRepository.FindByIdAsync("P1").Returns(new Part { PartNumber = "P1" });
            this.EmployeeRepository.FindByIdAsync(this.employeeId).Returns(new Employee());
            this.StoresService.ValidStockPool(Arg.Is<Part>(b => b.PartNumber == "P1"), Arg.Any<StockPool>())
                .Returns(new ProcessResult(false, "Stock pool is bad"));
            this.ReqStoredProcedures.CreateRequisitionLines(123, null)
                .Returns(new ProcessResult(true, "lines ok"));
            this.ReqStoredProcedures.DoRequisition(Arg.Any<int>(), null, this.employeeId)
                .Returns(new ProcessResult(true, "still ok"));
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet { PalletNumber = 123 });
            this.StockPoolRepository.FindByIdAsync("SP1").Returns(
                new StockPool("SP1", null, null, new AccountingCompany(), null, null, null, null, null, null));
            this.action = async () => await this.Sut.Validate(
                                          this.employeeId,
                                          TestFunctionCodes.Move.FunctionCode,
                                          "F",
                                          null,
                                          null,
                                          null,
                                          null,
                                          reference: null,
                                          comments: null,
                                          manualPick: null,
                                          fromStockPool: "SP1",
                                          toStockPool: "SP1",
                                          fromPalletNumber: 123,
                                          toPalletNumber: 123,
                                          fromLocationCode: null,
                                          toLocationCode: null,
                                          partNumber: "P1",
                                          quantity: this.quantity,
                                          fromState: "S1",
                                          toState: "S2",
                                          batchRef: null,
                                          batchDate: null,
                                          document1Line: null);
        }

        [Test]
        public async Task ShouldThrowError()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Stock pool is bad");
        }
    }
}
