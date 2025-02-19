namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenCancellingLine
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
                reference: null,
                comments: "Goodbye Reqs");
            this.sut.AddLine(new LineWithMoves(1, 1));
            this.sut.CancelLine(1, "reason", new Employee());
        }

        [Test]
        public void ShouldCancelLine()
        {
            this.sut.Lines.All(
                x => x.Cancelled == "Y"
                     && x.DateCancelled.HasValue
                     && x.CancelledBy != null
                     && x.CancelledReason == "reason").Should().BeTrue();
        }

        [Test]
        public void ShouldCancelHeader()
        {
            this.sut.Cancelled.Should().Be("Y");
            this.sut.DateCancelled.Should().NotBeNull();
        }

        [Test]
        public void ShouldCancelMoves()
        {
            this.sut.Lines
                .All(l => l.Moves.All(x => x.DateCancelled.HasValue)).Should().BeTrue();
        }
    }
}
