﻿namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;
    using System;
    using FluentAssertions;

    public class WhenCreatingAndInvalidTypeOfStock
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var site = new StorageSite { SiteCode = "TEST" };
            var area = new StorageArea { StorageAreaCode = "TEST" };
            var company = new AccountingCompany { Name = "TRENT" };
            this.action = () =>
            {
                _ = new StorageLocation(
                    1, "E-TESTY-TEST", "TEST LOCATION", site, area, company, "Y", "Y", "Y", "A", "Z");
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Cannot create Location - type of stock should be R, F or A");
        }
    }
}
