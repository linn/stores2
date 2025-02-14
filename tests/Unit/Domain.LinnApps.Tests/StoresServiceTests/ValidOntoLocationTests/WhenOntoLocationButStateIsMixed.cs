namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidOntoLocationTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenOntoLocationButStateIsMixed : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.StockService.GetStockLocators(this.Part.PartNumber, this.Location.LocationId, null)
                .Returns(new List<StockLocator> { new StockLocator { Id = 1, State = "FAIL" } });
            this.OnToState = new StockState("GOOD", "Good");

            this.Result = this.Sut.ValidOntoLocation(this.Part, this.Location, null, this.OnToState).Result;
        }

        [Test]
        public void ShouldGetStock()
        {
            this.StockService.Received().GetStockLocators(this.Part.PartNumber, this.Location.LocationId, null);
        }

        [Test]
        public void ShouldNotBeValid()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be($"Stock not in state {this.OnToState.State} already exists at this location");
        }
    }
}
