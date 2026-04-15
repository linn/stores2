namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingTotalVolume : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            // Line volume = H * D * W / 1,000,000 = 100 * 100 * 100 / 1,000,000 = 1.0
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { ContainerNumber = 1, ItemType = "C", Height = 100, Depth = 100, Width = 100 }
            };

            // Pallet volume = H * W * D = 10 * 20 * 50 = 10,000
            this.Sut.Pallets = new List<ConsignmentPallet>
            {
                new ConsignmentPallet { PalletNumber = 1, Height = 10, Width = 20, Depth = 50 }
            };
        }

        [Test]
        public void ShouldReturnSumOfBoxVolumesAndPalletVolumes()
        {
            this.Sut.TotalVolume().Should().Be(1.01m);
        }
    }
}
