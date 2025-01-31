namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;
    using System;

    public class WhenCreatingAndNoSiteSupplied
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var area = new StorageArea { StorageAreaCode = "TEST" };
            var company = new AccountingCompany { Name = "ME" };
            this.action = () =>
            {
                _ = new StorageLocation(
                    1, "E-TESTY-TEST", "TEST LOCATION", null, area, company, "Y", "Y", "Y", "A", "A", null, null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Location needs a site");
        }
    }
}
