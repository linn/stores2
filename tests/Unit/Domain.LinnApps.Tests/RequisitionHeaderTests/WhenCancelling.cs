namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenCancelling
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionHeader(
                new Employee(),
                new StoresFunction { FunctionCode = "F1" },
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                null,
                null,
                "Goodbye Reqs");
            this.sut.AddLine(new LineWithMoves(123, 1));
            this.sut.Cancel("reason", new Employee());
        }

        [Test]
        public void ShouldCancelHeader()
        {
            this.sut.Cancelled.Should().Be("Y");
            this.sut.DateCancelled.Should().NotBeNull();
        }

        [Test]
        public void ShouldCancelLines()
        {
            this.sut.Lines.All(
                x => x.Cancelled == "Y" 
                     && x.DateCancelled.HasValue 
                     && x.CancelledBy != null 
                     && x.CancelledReason == "reason").Should().BeTrue();
        }

        [Test]
        public void ShouldCancelMoves()
        {
            this.sut.Lines
                .All(l => l.Moves.All(x => x.DateCancelled.HasValue)).Should().BeTrue();
        }
    }
}
