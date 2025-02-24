namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidStockPoolTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Parts;

    using NUnit.Framework;

    public class WhenStockPoolIsValidForPart : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = new Part { PartNumber = "P1", AccountingCompanyCode = "AC1" };

            this.Result = this.Sut.ValidStockPool(this.Part, this.StockPool);
        }

        [Test]
        public void ShouldReturnTrue()
        {
            this.Result.Success.Should().BeTrue();
        }
    }
}
