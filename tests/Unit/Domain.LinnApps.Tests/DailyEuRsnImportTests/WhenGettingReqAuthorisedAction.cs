namespace Linn.Stores2.Domain.LinnApps.Tests.DailyEuRsnImportTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Reports;

    using NUnit.Framework;

    public class WhenGettingDimensions
    {
        private DailyEuRsnImportReport sut;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.sut = new DailyEuRsnImportReport { Width = 12, Height = 3, Depth = 4 };
            this.result = this.sut.GetDims();
        }

        [Test]
        public void ShouldReturnDimensions()
        {
            this.result.Should().Be("12 x 3 x 4");
        }
    }
}
