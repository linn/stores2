namespace Linn.Stores2.Domain.LinnApps.Tests.StorageSiteTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NUnit.Framework;

    public class WhenUpdatingAndNoDescriptionSupplied
    {
        private StorageSite sut;

        private Action act;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StorageSite("CODE", "DESC", "P");
            this.act = () => { this.sut.Update(null, null); };
        }

        [Test]
        public void ShouldThrow()
        {
            this.act.Should().Throw<InvalidEntityException>().WithMessage("Description must be populated");
        }
    }
}
