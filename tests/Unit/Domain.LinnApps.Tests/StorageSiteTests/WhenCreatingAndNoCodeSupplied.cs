namespace Linn.Stores2.Domain.LinnApps.Tests.StorageSiteTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class WhenCreatingAndNoCodeSupplied
    {
        private Action act;

        [SetUp]
        public void SetUp()
        {
            this.act = () => new StorageSite(string.Empty, "blah", "blah");
        }

        [Test]
        public void ShouldThrow()
        {
            this.act.Should().Throw<InvalidEntityException>().WithMessage("Code must be populated");
        }
    }
}
