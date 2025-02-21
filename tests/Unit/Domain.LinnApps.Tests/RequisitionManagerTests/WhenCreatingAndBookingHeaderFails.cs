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
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAndBookingHeaderFails : ContextBase
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
                new StoresFunction { FunctionCode = "FUNC" },
                "F",
                null,
                "REQ",
                new Department(),
                new Nominal(),
                part: new Part { PartNumber = "P1" },
                quantity: this.quantity);
            this.ReqRepository.FindByIdAsync(123).Returns(this.req);

            this.StoresService.ValidOntoLocation(
                Arg.Is<Part>(p => p.PartNumber == "P1"),
                Arg.Any<StorageLocation>(),
                Arg.Any<StoresPallet>(),
                Arg.Any<StockState>()).Returns(new ProcessResult(true, "ok"));
            this.ReqStoredProcedures.CreateRequisitionLines(123, null)
                .Returns(new ProcessResult(false, "lines not ok"));

            this.action = async () => await this.Sut.CheckAndBookRequisitionHeader(this.req);
        }

        [Test]
        public async Task ShouldThrowError()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage("lines not ok");
        }
    }
}
