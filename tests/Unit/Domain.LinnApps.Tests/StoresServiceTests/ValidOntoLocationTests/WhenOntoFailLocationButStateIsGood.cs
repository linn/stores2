namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidOntoLocationTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class WhenOntoFailLocationButStateIsGood : StoresServiceContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Location.StockState = "Q";
            this.OnToState = new StockState("STORES", "Good Stock");

            this.Result = this.Sut.ValidOntoLocation(this.Part, this.Location, null, this.OnToState).Result;
        }

        [Test]
        public void ShouldNotBeValid()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("Only uninspected/failed stock can be placed on this location");
        }
    }
}
