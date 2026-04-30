namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingTotalWeightWithMultipleBoxesPerLine : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            // Containers 1, 2, 3 share same weight/dims → one line with Count=3, Weight=4m → contributes 12m
            // Container 4 differs → one line with Count=1, Weight=6m → contributes 6m
            // Pallet weight = 5m
            // Total = 12 + 6 + 5 = 23m
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { ContainerNumber = 1, ItemType = "C", Weight = 4m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "C", Weight = 4m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "C", Weight = 4m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 4, ItemType = "C", Weight = 6m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 1, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 4, ItemType = "I", ItemDescription = "Widget B", Quantity = 1, MaybeHalfAPair = "N" }
            };

            this.Sut.Pallets = new List<ConsignmentPallet>
            {
                new ConsignmentPallet { PalletNumber = 1, Weight = 5 }
            };
        }

        [Test]
        public void ShouldMultiplyWeightByCountForEachLine()
        {
            this.Sut.TotalWeight().Should().Be(23m);
        }
    }
}
