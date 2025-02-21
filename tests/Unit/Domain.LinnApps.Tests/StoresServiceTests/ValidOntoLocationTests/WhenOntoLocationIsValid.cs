namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidOntoLocationTests
{
    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenOntoLocationIsValid : StoresServiceContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Result = this.Sut.ValidOntoLocation(this.Part, this.Location, null, this.OnToState).Result;
        }

        [Test]
        public void ShouldGetStock()
        {
            this.StockService.Received().GetStockLocators(this.Part.PartNumber, this.Location.LocationId, null);
        }

        [Test]
        public void ShouldReturnTrue()
        {
            this.Result.Success.Should().BeTrue();
        }
    }
}
