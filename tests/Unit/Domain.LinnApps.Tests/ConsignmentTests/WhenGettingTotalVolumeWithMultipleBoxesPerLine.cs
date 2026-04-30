namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingTotalVolumeWithMultipleBoxesPerLine : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            // Containers 1, 2, 3 share same dims → one line with Count=3
            // Volume per box = 100 * 100 * 100 / 1,000,000 = 1.0m³  → contributes 3 * 1.0 = 3.0m³
            // Container 4 has different dims → one line with Count=1
            // Volume per box = 200 * 100 * 100 / 1,000,000 = 2.0m³  → contributes 1 * 2.0 = 2.0m³
            // Pallet volume = 10 * 20 * 50 = 10,000 cm³ / 1,000,000 = 0.01m³
            // Total = 3.0 + 2.0 + 0.01 = 5.01m³
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { ContainerNumber = 1, ItemType = "C", Weight = 2.5m, Height = 100, Depth = 100, Width = 100 },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "C", Weight = 2.5m, Height = 100, Depth = 100, Width = 100 },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "C", Weight = 2.5m, Height = 100, Depth = 100, Width = 100 },
                new ConsignmentItem { ContainerNumber = 4, ItemType = "C", Weight = 2.5m, Height = 200, Depth = 100, Width = 100 },
                new ConsignmentItem { ContainerNumber = 1, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 4, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" }
            };

            this.Sut.Pallets = new List<ConsignmentPallet>
            {
                new ConsignmentPallet { PalletNumber = 1, Height = 10, Width = 20, Depth = 50 }
            };
        }

        [Test]
        public void ShouldMultiplyVolumeByCountForEachLine()
        {
            this.Sut.TotalVolume().Should().Be(5.01m);
        }
    }
}
