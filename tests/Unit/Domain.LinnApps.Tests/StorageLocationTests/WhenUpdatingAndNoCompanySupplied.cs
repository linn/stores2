namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;
    using System;
    using FluentAssertions;

    public class WhenUpdatingAndNoCompanySupplied
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var site = new StorageSite { SiteCode = "TEST" };
            var area = new StorageArea { StorageAreaCode = "TEST" };
            var location = new StorageLocation();
            this.action = () =>
            {
               location.Update("New Description", null, "Y", "Y", "Y", "A", "A", null, null, null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Location needs an accounting company");
        }
    }
}
