namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingContainerDescriptionWithNullItems : ContextBase
    {
        private string result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = null;
            this.result = this.Sut.GetContainerDescription(1);
        }

        [Test]
        public void ShouldReturnEmptyString()
        {
            this.result.Should().BeEmpty();
        }
    }
}
