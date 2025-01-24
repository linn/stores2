namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenCancelling
    {
        private RequisitionHeader Sut;

        [SetUp]
        public void SetUp()
        {
            this.Sut = new ReqWithLines(123, new StoresFunctionCode { FunctionCode = "CODE" });
            this.Sut.Cancel("reason", new Employee());
        }

        [Test]
        public void ShouldCancelHeader()
        {
            this.Sut.Cancelled.Should().Be("Y");
            this.Sut.DateCancelled.Should().NotBeNull();
        }

        [Test]
        public void ShouldCancelLines()
        {
            this.Sut.Lines.All(
                x => x.Cancelled == "Y" 
                     && x.DateCancelled.HasValue 
                     && x.CancelledBy != null 
                     && x.CancelledReason == "reason").Should().BeTrue();
        }

        [Test]
        public void ShouldCancelMoves()
        {
            this.Sut.Lines
                .All(l => l.Moves.All(x => x.DateCancelled.HasValue)).Should().BeTrue();
        }
    }
}
