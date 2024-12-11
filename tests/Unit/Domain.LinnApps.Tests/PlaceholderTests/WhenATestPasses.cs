namespace Linn.Stores2.Domain.LinnApps.Tests.PlaceholderTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenATestPasses
    {
        private bool result;
        
        [SetUp]
        public void SetUp()
        {
            this.result = true;
        }

        [Test]
        public void ShouldBeAThing()
        {
            this.result.Should().BeTrue();
        }
    }
}