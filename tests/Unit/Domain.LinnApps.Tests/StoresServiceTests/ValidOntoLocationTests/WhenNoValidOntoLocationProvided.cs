namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidOntoLocationTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenNoValidOntoLocationProvided : StoresServiceContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Result = this.Sut.ValidOntoLocation(this.Part, null, null, this.OnToState).Result;
        }

        [Test]
        public void ShouldNotBeValid()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("No valid onto location or state provided");
        }
    }
}
