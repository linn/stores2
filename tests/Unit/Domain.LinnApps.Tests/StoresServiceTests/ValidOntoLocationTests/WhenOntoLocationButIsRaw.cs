namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidOntoLocationTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenOntoLocationButIsRaw : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part.RawOrFinished = "R";
            this.Location.TypeOfStock = "F";

            this.Result = this.Sut.ValidOntoLocation(this.Part, this.Location, null, this.OnToState).Result;
        }

        [Test]
        public void ShouldNotBeValid()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("Location/Pallet is for F but part P1 is R");
        }
    }
}
