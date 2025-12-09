namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.DefaultBookInLocationTests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenK1Default : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.PartStorageTypeRepository.FindByAsync(Arg.Any<Expression<Func<PartStorageType, bool>>>())
                .Returns(new PartStorageType { StorageTypeCode = "K1TYPE" });
            this.StorageLocationRepository.FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>())
                .Returns(this.DefaultForK1);

            this.LocationResult = await this.Sut.DefaultBookInLocation(this.Part.PartNumber);
        }

        [Test]
        public void ShouldReturnCorrectLocation()
        {
            this.LocationResult.LocationCode.Should().Be(this.DefaultForK1.LocationCode);
            this.LocationResult.LocationId.Should().Be(this.DefaultForK1.LocationId);
        }
    }
}
