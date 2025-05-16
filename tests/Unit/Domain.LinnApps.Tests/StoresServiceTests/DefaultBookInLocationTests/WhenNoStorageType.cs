namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.DefaultBookInLocationTests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNoStorageType : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.PartStorageTypeRepository.FindByAsync(Arg.Any<Expression<Func<PartsStorageType, bool>>>())
                .Returns((PartsStorageType)null);

            this.LocationResult = await this.Sut.DefaultBookInLocation(this.Part.PartNumber);
        }

        [Test]
        public void ShouldNotLookUpLocation()
        {
            this.StorageLocationRepository.DidNotReceive()
                .FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>());
        }

        [Test]
        public void ShouldReturnNoLocation()
        {
            this.LocationResult.Should().BeNull();
        }
    }
}
