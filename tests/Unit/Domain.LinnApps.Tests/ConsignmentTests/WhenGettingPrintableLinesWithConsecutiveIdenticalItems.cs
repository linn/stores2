namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NUnit.Framework;

    public class WhenGettingPrintableLinesWithConsecutiveIdenticalItems : ContextBase
    {
        private IList<ConsignmentPrintLine> result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { ContainerNumber = 1, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 1, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" }
            };

            this.result = this.Sut.GetPrintableLines().ToList();
        }

        [Test]
        public void ShouldReturnOneLine()
        {
            this.result.Should().HaveCount(1);
        }

        [Test]
        public void ShouldSpanAllContainerNumbers()
        {
            this.result[0].LowValue.Should().Be(1);
            this.result[0].HighValue.Should().Be(3);
        }

        [Test]
        public void ShouldCountAllItems()
        {
            this.result[0].Count.Should().Be(3);
        }
    }
}
