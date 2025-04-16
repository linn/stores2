namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPartNumberChangeTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using NUnit.Framework;

    public class WhenOldPartIsNotLive : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = new Part { PartNumber = "ADIKT", ProductAnalysisCode = "ADIKT" };
            this.NewPart = new Part { PartNumber = "ADIKT/X", ProductAnalysisCode = "ADIKT", DateLive = DateTime.Now };

            this.Result = this.Sut.ValidPartNumberChange(this.Part, this.NewPart);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("Old part number ADIKT is not live");
        }
    }
}
