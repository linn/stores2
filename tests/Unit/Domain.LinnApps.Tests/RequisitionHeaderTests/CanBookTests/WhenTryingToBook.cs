namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests.CanBookTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenTryingToBook : CanBookContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Result = this.Sut.RequisitionCanBeBooked();
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.Result.Success.Should().BeTrue();
            this.Result.Message.Should().Be("Quantity on req 0 is 2 and the lines match.");
        }
    }
}
