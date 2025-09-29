namespace Linn.Stores2.Domain.LinnApps.Tests.StockLocatorTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;

    public class WhenGettingLabourHours
    {
        private StockLocator sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StockLocator
            {
                Part = new Part
                {
                    PartNumber = "PART",
                    Bom = new Bom()
                    {
                        LabourTimeMins = 60,
                        TotalLabourTimeMins = 60
                    }
                },
                Quantity = 1
            };
        }

        [Test]
        public void ShouldMakeSummary()
        {
            this.sut.LabourHours().Should().Be(1);
        }
    }
}
