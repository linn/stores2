namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPartNumberChangeTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using NUnit.Framework;

    public class WhenNewPartIsNotLive : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = new Part { PartNumber = "ADIKT", ProductAnalysisCode = "ADIKT", DateLive = DateTime.Now };
            this.NewPart = new Part { PartNumber = "ADIKT/X", ProductAnalysisCode = "ADIKT" };

            this.Result = this.Sut.ValidPartNumberChange(this.Part, this.NewPart);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("New part number ADIKT/X is not live");
        }
    }
}
