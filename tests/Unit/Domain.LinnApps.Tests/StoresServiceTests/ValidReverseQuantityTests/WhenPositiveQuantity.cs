namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidReverseQuantityTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenPositiveQuantity : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.Result = await this.Sut.ValidReverseQuantity(this.OriginalReqNumber, 4m);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("A reverse quantity must be negative");
        }
    }
}
