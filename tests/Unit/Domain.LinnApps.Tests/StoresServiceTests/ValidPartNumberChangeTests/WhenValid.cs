namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPartNumberChangeTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using NUnit.Framework;

    public class WhenValid : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = new Part
                            {
                                PartNumber = "ADIKT",
                                DateLive = DateTime.Now,
                                ProductAnalysisCode = "ADIKT",
                                BaseUnitPrice = 1000
                            };
            this.NewPart = new Part
                               {
                                   PartNumber = "ADIKT/X",
                                   DateLive = DateTime.Now,
                                   ProductAnalysisCode = "ADIKT",
                                   BaseUnitPrice = 1099
                               };

            this.Result = this.Sut.ValidPartNumberChange(this.Part, this.NewPart);
        }

        [Test]
        public void ShouldReturnTrue()
        {
            this.Result.Success.Should().BeTrue();
        }
    }
}
