namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidReverseQuantityTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidQuantity : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            var req = new TestData.Requisitions.ReqWithReqNumber(
                this.OriginalReqNumber,
                new Employee(),
                new StoresFunction("SF"),
                null,
                null,
                null,
                null,
                null,
                quantity: 4m);
            this.RequisitionRepository.FindByIdAsync(this.OriginalReqNumber).Returns(req);

            this.Result = await this.Sut.ValidReverseQuantity(this.OriginalReqNumber, -4m);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Result.Success.Should().BeTrue();
            this.Result.Message.Should().Be($"Reverse quantity of -4 for req {this.OriginalReqNumber} is valid");
        }
    }
}
