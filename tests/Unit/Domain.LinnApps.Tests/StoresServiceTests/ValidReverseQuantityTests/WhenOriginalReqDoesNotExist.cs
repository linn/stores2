namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidReverseQuantityTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenOriginalReqDoesNotExist : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.RequisitionRepository.FindByIdAsync(this.OriginalReqNumber).Returns((RequisitionHeader)null);

            this.Result = await this.Sut.ValidReverseQuantity(this.OriginalReqNumber, -4m);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be($"Original requisition {this.OriginalReqNumber} does not exist");
        }
    }
}
