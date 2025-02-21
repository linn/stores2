namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidStockPoolTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Parts;

    using NUnit.Framework;

    public class WhenStockPoolIsNotValidForPart : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Part = new Part { PartNumber = "P1", AccountingCompanyCode = "AC2" };

            this.Result = this.Sut.ValidStockPool(this.Part, this.StockPool);
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("Stock Pool SP1 is for AC1 and is not valid for part P1");
        }
    }
}
