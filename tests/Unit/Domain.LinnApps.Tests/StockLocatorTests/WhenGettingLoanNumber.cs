namespace Linn.Stores2.Domain.LinnApps.Tests.StockLocatorTests
{
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;
    using FluentAssertions;

    public class WhenGettingLoanNumber
    {
        private StockLocator sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StockLocator
            {
                Part = new Part
                {
                    PartNumber = "PARTNOBOM"
                },
                Quantity = 1,
                BatchRef = "L12345"
            };
        }

        [Test]
        public void ShouldMakeSummary()
        {
            this.sut.LoanNumber().Should().Be(12345);
        }
    }
}
