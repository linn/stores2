namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingContainerDescriptionFilteringByItemType : ContextBase
    {
        private string result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "I",
                    Quantity = 5,
                    ItemDescription = "Item Type I",
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "S",
                    Quantity = 3,
                    ItemDescription = "Item Type S",
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "X",
                    Quantity = 10,
                    ItemDescription = "Item Type X",
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "Y",
                    Quantity = 7,
                    ItemDescription = "Item Type Y",
                    MaybeHalfAPair = "N"
                }
            };

            this.result = this.Sut.GetContainerDescription(1);
        }

        [Test]
        public void ShouldIncludeItemTypeI()
        {
            this.result.Should().Contain("5 Item Type I");
        }

        [Test]
        public void ShouldIncludeItemTypeS()
        {
            this.result.Should().Contain("3 Item Type S");
        }

        [Test]
        public void ShouldNotIncludeOtherItemTypes()
        {
            this.result.Should().NotContain("Item Type X");
            this.result.Should().NotContain("Item Type Y");
        }
    }
}
