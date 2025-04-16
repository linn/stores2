using FluentAssertions;
using Linn.Stores2.Domain.LinnApps.Parts;
using NUnit.Framework;
using System;

namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPartNumberChangeTests
{
    public class WhenPriceDiffIsTooMuch: ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = new Part { PartNumber = "ADIKT", DateLive = DateTime.Now, ProductAnalysisCode = "ADIKT", BaseUnitPrice = 1000 };
            this.NewPart = new Part { PartNumber = "ADIKT PRO/2", DateLive = DateTime.Now, ProductAnalysisCode = "ADIKT", BaseUnitPrice = 555 };

            this.Result = this.Sut.ValidPartNumberChange(this.Part, this.NewPart);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("Price change of 180% not allowed");
        }
    }
}
