namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingContainerDescription : ContextBase
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
                    ItemDescription = "Widget A",
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "S",
                    Quantity = 3,
                    ItemDescription = "Widget B",
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 2,
                    ItemType = "I",
                    Quantity = 10,
                    ItemDescription = "Widget C",
                    MaybeHalfAPair = "N"
                }
            };

            this.result = this.Sut.GetContainerDescription(1);
        }

        [Test]
        public void ShouldReturnCorrectDescription()
        {
            this.result.Should().Contain("5 Widget A");
            this.result.Should().Contain("3 Widget B");
        }

        [Test]
        public void ShouldNotIncludeItemsFromOtherContainers()
        {
            this.result.Should().NotContain("Widget C");
        }

        [Test]
        public void ShouldJoinWithCommas()
        {
            this.result.Should().Contain(", ");
        }
    }
}
