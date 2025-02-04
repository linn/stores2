using Linn.Stores2.Domain.LinnApps.Exceptions;
using Linn.Stores2.Domain.LinnApps.Stock;
using NUnit.Framework;
using System;
using FluentAssertions;

namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    public class WhenCreatingAndNoCompanySupplied
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var site = new StorageSite { SiteCode = "TEST" };
            var area = new StorageArea { StorageAreaCode = "TEST" };
            this.action = () =>
            {
                _ = new StorageLocation(
                    1, "E-TESTY-TEST", "TEST LOCATION", site, area, null, "Y", "Y", "Y", "A", "A", null, null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Location needs an accounting company");
        }
    }
}
