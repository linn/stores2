
namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;
    using System;
    using FluentAssertions;

    public class WhenCreatingAndInvalidLocationCodePrefix
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var site = new StorageSite { SiteCode = "TEST", SitePrefix = "T" };
            var area = new StorageArea { StorageAreaCode = "TEST", AreaPrefix = "TE" };
            var company = new AccountingCompany { Name = "TRENT" };
            this.action = () =>
            {
                _ = new StorageLocation(
                    1, "E-TESTY-TEST", "TEST LOCATION", site, area, company, "Y", "Y", "Y", "A", "A", null, null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Cannot create Location - location code should start with T-TE-");
        }
    }
}
