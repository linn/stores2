namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPoQcBatchTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using NUnit.Framework;

    public class WhenBatchRefDoesNotMatchOrder : StoresServiceContextBase
    {
        private ProcessResult result;
        
        [SetUp]
        public async Task Setup()
        {
            this.result = await this.Sut.ValidPoQcBatch("P123456", 789012, 1);
        }

        [Test]
        public void ShouldReturnError()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should()
                .Be("You are trying to pass batch P123456 for payment against the wrong Order Number: 789012");
        }
    }
}
