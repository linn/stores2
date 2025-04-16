namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPartNumberChangeTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using NUnit.Framework;

    public class WhenDifferentProductAnalysisCodes : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = new Part { PartNumber = "ADIKT", DateLive = DateTime.Now, ProductAnalysisCode = "ADIKT" };
            this.NewPart = new Part { PartNumber = "KEEL", DateLive = DateTime.Now, ProductAnalysisCode = "LP12" };

            this.Result = this.Sut.ValidPartNumberChange(this.Part, this.NewPart);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be($"Old part is for product group ADIKT new part is for product group LP12");
        }
    }
}
