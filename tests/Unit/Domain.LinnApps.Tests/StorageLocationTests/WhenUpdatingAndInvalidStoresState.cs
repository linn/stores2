namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class WhenUpdatingAndInvalidStoresState
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
                location.Update(
                    "New Description",
                    new AccountingCompany(),
                    "Y",
                    "Y",
                    "Y",
                    "Z",
                    "A",
                    null,
                    null,
                    null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Cannot update Location - stock state should be I, Q or A");
        }
    }
}
