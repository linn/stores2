namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class WhenCreatingAndNoAreaSupplied
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var site = new StorageSite { SiteCode = "TEST" };
            var company = new AccountingCompany { Name = "ME" };
            this.action = () =>
            {
                _ = new StorageLocation(
                    1,
                    "E-TESTY-TEST",
                    "TEST LOCATION",
                    site,
                    null,
                    company,
                    "Y",
                    "Y",
                    "Y",
                    "Y",
                    "A",
                    "A",
                    null,
                    null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Location needs an area");
        }
    }
}
