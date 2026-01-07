namespace Linn.Stores2.Domain.LinnApps.Tests.TqmsDataTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Parts;
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
                TotalQty = 1,
                Part = new Part
                {
                    PartNumber = "URIKA",
                    Bom = new Bom
                    {
                        TotalLabourTimeMins = 9.9m
                    }
                }
            };
        }

        [Test]
        public void ShouldSetOntoFields()
        {
            this.sut.LabourHours().Should().Be(0.165m);
        }
    }
}
