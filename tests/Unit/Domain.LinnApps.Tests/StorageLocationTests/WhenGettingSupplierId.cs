namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;

    public class WhenGettingSupplierId
    {
        private StorageLocation sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StorageLocation
            {
                LocationCode = "S-SU-113021"
            };
        }

        [Test]
        public void ShouldGetSupplierId()
        {
            this.sut.GetSupplierId().Should().Be(113021);
        }
    }
}
