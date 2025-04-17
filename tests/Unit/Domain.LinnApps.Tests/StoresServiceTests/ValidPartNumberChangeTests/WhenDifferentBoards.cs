namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPartNumberChangeTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using NUnit.Framework;

    public class WhenDifferentBoards : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = new Part
                            {
                                PartNumber = "PCAS 100/L1R1",
                                DateLive = DateTime.Now,
                                ProductAnalysisCode = "ELECTRONIC"
                            };
            this.NewPart = new Part
                               {
                                   PartNumber = "PCAS 656/L2R2",
                                   DateLive = DateTime.Now,
                                   ProductAnalysisCode = "ELECTRONIC"
                               };

            this.Result = this.Sut.ValidPartNumberChange(this.Part, this.NewPart);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be($"Old part PCAS 100/L1R1 is a different board from PCAS 656/L2R2");
        }
    }
}
