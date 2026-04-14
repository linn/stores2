namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NUnit.Framework;

    public class WhenGettingPrintableLines : ContextBase
    {
        private IList<ConsignmentPrintLine> result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { ContainerNumber = 1, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "C", Weight = 2.5m, Height = 10, Depth = 20, Width = 30 },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "C", Weight = 5.0m, Height = 15, Depth = 25, Width = 35 },
                new ConsignmentItem { ContainerNumber = 1, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 2, ItemType = "I", ItemDescription = "Widget A", Quantity = 1, MaybeHalfAPair = "N" },
                new ConsignmentItem { ContainerNumber = 3, ItemType = "I", ItemDescription = "Widget B", Quantity = 1, MaybeHalfAPair = "N" }
            };

            this.result = this.Sut.GetPrintableLines().ToList();
        }

        [Test]
        public void ShouldReturnOneLinePerGroup()
        {
            this.result.Should().HaveCount(2);
        }

        [Test]
        public void ShouldSetLowAndHighContainerNumbersForFirstGroup()
        {
            this.result[0].LowValue.Should().Be(1);
            this.result[0].HighValue.Should().Be(2);
        }

        [Test]
        public void ShouldSetCountForFirstGroup()
        {
            this.result[0].Count.Should().Be(2);
        }

        [Test]
        public void ShouldSetDescriptionForFirstGroup()
        {
            this.result[0].ItemDescription.Should().Be("1 Widget A");
        }

        [Test]
        public void ShouldSetWeightForFirstGroup()
        {
            this.result[0].Weight.Should().Be(2.5m);
        }

        [Test]
        public void ShouldSetDimsForFirstGroup()
        {
            this.result[0].Dims.Should().Be("10 x 30 x 20 cm");
        }

        [Test]
        public void ShouldSetDescriptionForSecondGroup()
        {
            this.result[1].ItemDescription.Should().Be("1 Widget B");
        }

        [Test]
        public void ShouldSetCountForSecondGroup()
        {
            this.result[1].Count.Should().Be(1);
        }

        [Test]
        public void ShouldSetLowAndHighContainerNumbersForSecondGroup()
        {
            this.result[1].LowValue.Should().Be(3);
            this.result[1].HighValue.Should().Be(3);
        }
    }
}
