namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingTotalNumberOfBoxesWithNoItems : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = null;
        }

        [Test]
        public void ShouldReturnZero()
        {
            this.Sut.TotalNumberOfBoxes().Should().Be(0);
        }
    }
}
