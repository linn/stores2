namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NUnit.Framework;

    public class WhenUpdatingAndInvalidSalesKittable 
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var location = new StorageLocation();
            this.action = () =>
            {
                location.Update(
                    "New Description",
                    new AccountingCompany(),
                    "Y",
                    "Y",
                    "Z",
                    "Y",
                    "A",
                    "A",
                    null,
                    null,
                    null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Cannot update Location - sales kittable should be Y, N or blank");
        }
    }
}
