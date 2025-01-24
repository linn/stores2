namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenCancelling
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new ReqWithLines(123, new StoresFunctionCode { FunctionCode = "CODE" });
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
