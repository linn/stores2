namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPoQcBatchTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using NUnit.Framework;

    public class WhenNothingInQcForOrder : StoresServiceContextBase
    {
        private ProcessResult result;
        
        [SetUp]
        public async Task Setup()
        {
            this.result = await this.Sut.ValidPoQcBatch("P123456", 123456, 1);
        }

        [Test]
        public void ShouldReturnError()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should()
                .Be("Nothing found to pass for payment - Check order 123456 has been booked in.");
        }
    }
}
