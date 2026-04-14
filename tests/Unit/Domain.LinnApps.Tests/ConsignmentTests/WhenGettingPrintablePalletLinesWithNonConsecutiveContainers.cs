namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NUnit.Framework;

    public class WhenGettingPrintablePalletLinesWithNonConsecutiveContainers : ContextBase
    {
        private IList<ConsignmentPrintLine> result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { ContainerNumber = 1, PalletNumber = 1, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 2, PalletNumber = 1, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 4, PalletNumber = 1, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 1, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 4, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" }
            };

            this.result = this.Sut.GetPrintablePalletLines(1).ToList();
        }

        [Test]
        public void ShouldReturnTwoLines()
        {
            this.result.Should().HaveCount(2);
        }

        [Test]
        public void ShouldSetRangeForConsecutiveGroup()
        {
            this.result[0].LowValue.Should().Be(1);
            this.result[0].HighValue.Should().Be(2);
        }

        [Test]
        public void ShouldSetRangeForNonConsecutiveContainer()
        {
            this.result[1].LowValue.Should().Be(4);
            this.result[1].HighValue.Should().Be(4);
        }

        [Test]
        public void ShouldSetCountForConsecutiveGroup()
        {
            this.result[0].Count.Should().Be(2);
        }

        [Test]
        public void ShouldSetCountForNonConsecutiveContainer()
        {
            this.result[1].Count.Should().Be(1);
        }
    }
}
