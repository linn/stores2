namespace Linn.Stores2.Domain.LinnApps.Tests.StorageLocationTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class WhenUpdatingAndNoCompanySupplied
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var location = new StorageLocation();
            this.action = () =>
            {
                location.Update("New Description", null, "Y", "Y", "Y", "Y", "A", "A", null, null, null);
            };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<StorageLocationException>().WithMessage("Location needs an accounting company");
        }
    }
}
