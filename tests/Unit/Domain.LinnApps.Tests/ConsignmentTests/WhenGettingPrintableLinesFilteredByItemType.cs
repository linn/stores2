namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NUnit.Framework;

    public class WhenGettingPrintableLinesFilteredByItemType : ContextBase
    {
        private IList<ConsignmentPrintLine> result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "C",
                    ItemDescription = "Box",
                    Weight = 3.0m,
                    Height = 10,
                    Depth = 20,
                    Width = 30,
                    PalletNumber = null
                },
                new ConsignmentItem
                {
                    ContainerNumber = 2,
                    ItemType = "C",
                    ItemDescription = "Box",
                    Weight = 3.0m,
                    Height = 10,
                    Depth = 20,
                    Width = 30,
                    PalletNumber = 1
                },
                new ConsignmentItem
                {
                    ContainerNumber = null,
                    ItemType = "I",
                    ItemDescription = "Loose Item",
                    Weight = 1.0m,
                    Height = 5,
                    Depth = 5,
                    Width = 5,
                    PalletNumber = null
                },
                new ConsignmentItem
                {
                    ContainerNumber = 3,
                    ItemType = "I",
                    ItemDescription = "Item In Container",
                    Weight = 1.0m,
                    Height = 5,
                    Depth = 5,
                    Width = 5,
                    PalletNumber = null
                },
                new ConsignmentItem
                {
                    ContainerNumber = null,
                    ItemType = "X",
                    ItemDescription = "Unknown Type",
                    Weight = 1.0m,
                    Height = 5,
                    Depth = 5,
                    Width = 5,
                    PalletNumber = null
                }
            };

            this.result = this.Sut.GetPrintableLines().ToList();
        }

        [Test]
        public void ShouldExcludeContainerItemsOnAPallet()
        {
            this.result.Should().NotContain(l => l.LowValue == 2);
        }

        [Test]
        public void ShouldExcludeItemTypeIItemsInAContainer()
        {
            this.result.Should().NotContain(l => l.ItemDescription == "Item In Container");
        }

        [Test]
        public void ShouldExcludeUnknownItemTypes()
        {
            this.result.Should().NotContain(l => l.ItemDescription == "Unknown Type");
        }

        [Test]
        public void ShouldIncludeContainerItemsNotOnAPallet()
        {
            this.result.Should().Contain(l => l.LowValue == 1);
        }

        [Test]
        public void ShouldIncludeLooseItemTypeIItems()
        {
            this.result.Should().Contain(l => l.ItemDescription == "Loose Item");
        }
    }
}
