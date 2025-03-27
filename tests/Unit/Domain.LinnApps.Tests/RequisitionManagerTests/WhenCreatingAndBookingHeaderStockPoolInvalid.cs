namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAndBookingHeaderStockPoolInvalid : ContextBase
    {
        private RequisitionHeader req;

        private decimal quantity;

        private int employeeId;

        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.employeeId = 567;
            this.quantity = 49;
            this.req = new ReqWithReqNumber(
                123,
                new Employee { Id = this.employeeId },
                TestFunctionCodes.Move,
                "F",
                null,
                "REQ",
                new Department(),
                new Nominal(),
                part: new Part { PartNumber = "P1" },
                quantity: this.quantity,
                fromState: "S1",
                toState: "S2");
            this.ReqRepository.FindByIdAsync(Arg.Any<int>()).Returns(this.req);
            this.StateRepository.FindByIdAsync("S2")
                .Returns(new StockState("S2", "S2 desc"));

            this.StoresService.ValidOntoLocation(
                Arg.Is<Part>(p => p.PartNumber == "P1"),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>())
                .Returns(new ProcessResult(true, "ok"));
            this.StoresService.ValidStockPool(Arg.Is<Part>(b => b.PartNumber == "P1"), Arg.Any<StockPool>())
                .Returns(new ProcessResult(false, "Stock pool is bad"));
            this.ReqStoredProcedures.CreateRequisitionLines(123, null)
                .Returns(new ProcessResult(true, "lines ok"));
            this.ReqStoredProcedures.CanBookRequisition(123, null, this.quantity)
                .Returns(new ProcessResult(true, "can book ok"));
            this.ReqStoredProcedures.DoRequisition(Arg.Any<int>(), null, this.employeeId)
                .Returns(new ProcessResult(true, "still ok"));

            this.action = async () => await this.Sut.CheckAndBookRequisitionHeader(this.req);
        }

        [Test]
        public async Task ShouldThrowError()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("Stock pool is bad");
        }
    }
}
