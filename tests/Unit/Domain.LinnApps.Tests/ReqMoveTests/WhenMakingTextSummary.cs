namespace Linn.Stores2.Domain.LinnApps.Tests.ReqMoveTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenMakingTextSummary
    {
        private ReqMove sut;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new MoveWithLocation(
                new StockLocator { StorageLocation = new StorageLocation { LocationCode = "E-SMT-3" } },
                new StorageLocation { LocationCode = "E-K2-34", LocationId = 4989 },
                40);

            this.result = this.sut.TextSummary();
        }

        [Test]
        public void ShouldMakeSummary()
        {
            this.result.Should().Be("40 From E-SMT-3 To E-K2-34");
        }
    }
}
