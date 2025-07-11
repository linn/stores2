namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class WhenCreatingAndInvalidStoreState
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var site = new StorageSite ("SUPSTORES", "SUPPLIER STORES", null);
            var area = new StorageArea { StorageAreaCode = "TEST" };
            var company = new AccountingCompany { Name = "TRENT" };
            this.action = () =>
            {
                _ = new StorageLocation(
                    1,
                    "E-TESTY-TEST",
                    "TEST LOCATION",
                    site,
                    area,
                    company,
                    "Y",
                    "Y",
                    "Y",
                    "Y",
                    "Z",
                    "A",
                    null,
                    null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Cannot create Location - stock state should be I, Q or A");
        }
    }
}
