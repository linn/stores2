namespace Linn.Stores2.Domain.LinnApps.Tests.ReqMoveTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenMakingPalletTextSummary
    {
        private ReqMove sut;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new MoveWithPallet(
                new StockLocator { PalletNumber = 1000 },
                1234,
                50);

            this.result = this.sut.TextSummary();
        }

        [Test]
        public void ShouldMakeSummary()
        {
            this.result.Should().Be("50 From Pallet 1000 To Pallet 1234");
        }
    }
}
