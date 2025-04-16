namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidOntoLocationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenKardexLocationOccupiedByDifferentPartAndHasStorageType
        : StoresServiceContextBase
    {
        private ProcessResult result;

        [SetUp]
        public async Task SetUp()
        {
            this.Part.RawOrFinished = "R";
            var kardexLocWithNoStorageType = new StorageLocation(
                123,
                "E-K1-123",
                "DESC",
                new StorageSite(),
                new StorageArea(),
                new AccountingCompany(),
                "Y",
                "N",
                "N",
                "I",
                "R",
                new StockPool(),
                new StorageType("K106", "DESC"));
            this.StockLocatorRepository.FilterByAsync(Arg.Any<Expression<Func<StockLocator, bool>>>()).Returns(
                new List<StockLocator> { new StockLocator { PartNumber = "OTHER" } });
            this.result = await this.Sut.ValidOntoLocation(
                              this.Part, kardexLocWithNoStorageType, null, this.OnToState);
        }

        [Test]
        public void ShouldReturnFailure()
        {
            this.result.Success.Should().Be(false);
            this.result.Message.Should().Be("Part OTHER already at this location. Cannot mix parts since kardex location has storage type K106");
        }
    }
}
