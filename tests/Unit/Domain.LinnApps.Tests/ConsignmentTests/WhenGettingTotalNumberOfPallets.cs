namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingTotalNumberOfPallets : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Pallets = new List<ConsignmentPallet>
            {
                new ConsignmentPallet { PalletNumber = 1 },
                new ConsignmentPallet { PalletNumber = 2 }
            };
        }

        [Test]
        public void ShouldReturnPalletCount()
        {
            this.Sut.TotalNumberOfPallets().Should().Be(2);
        }
    }
}
