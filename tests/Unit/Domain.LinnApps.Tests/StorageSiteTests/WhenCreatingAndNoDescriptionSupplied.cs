namespace Linn.Stores2.Domain.LinnApps.Tests.StorageSiteTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class WhenCreatingAndNoDescriptionSupplied
    {
        private Action act;

        [SetUp]
        public void SetUp()
        {
            this.act = () => new StorageSite("BLAH", string.Empty, "blah");
        }

        [Test]
        public void ShouldThrow()
        {
            this.act.Should().Throw<InvalidEntityException>().WithMessage("Description must be populated");
        }
    }
}
