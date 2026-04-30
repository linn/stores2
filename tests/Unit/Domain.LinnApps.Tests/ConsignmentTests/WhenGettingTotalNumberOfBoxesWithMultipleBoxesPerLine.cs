namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingTotalNumberOfBoxesWithMultipleBoxesPerLine : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            // Containers 1, 2, 3 share the same description/weight/dims → one line with Count=3
            // Container 4 has different weight → one line with Count=1
            // Total boxes = 3 + 1 = 4
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { ContainerNumber = 1, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 4, ItemType = "C", Weight = 5.0m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 1, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 4, ItemType = "I", ItemDescription = "Widget B", Quantity = 1, MaybeHalfAPair = "N" }
            };
        }

        [Test]
        public void ShouldSumCountsAcrossAllLines()
        {
            this.Sut.TotalNumberOfBoxes().Should().Be(4);
        }
    }
}
