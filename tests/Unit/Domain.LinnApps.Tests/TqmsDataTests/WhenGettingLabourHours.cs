namespace Linn.Stores2.Domain.LinnApps.Tests.TqmsDataTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;

    public class WhenGettingLabourHours
    {
        private TqmsData sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new TqmsData
            {
                TotalQty = 5,
                Part = new Parts.Part
                {
                    PartNumber = "URIKA",
                    Bom = new Parts.Bom
                    {
                        TotalLabourTimeMins = 123.9m
                    }
                }
            };
        }

        [Test]
        public void ShouldSetOntoFields()
        {
            this.sut.LabourHours().Should().Be(10.325m);
        }
    }
}
