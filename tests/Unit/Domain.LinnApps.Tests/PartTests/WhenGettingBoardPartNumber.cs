namespace Linn.Stores2.Domain.LinnApps.Tests.PartTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using NUnit.Framework;

    public class WhenGettingBoardPartNumber : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new Part
            {
                PartNumber = "PCAS 100/L1R1"
            };
        }

        [Test]
        public void ShouldReturnBoardNumber()
        {
            this.Sut.BoardNumber().Should().Be("100");
        }
    }
}
