namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingContainerDescriptionWithGroupedItems : ContextBase
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
                    Quantity = 3,
                    ItemDescription = "Widget A",
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "I",
                    Quantity = 2,
                    ItemDescription = "Widget A",
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "S",
                    Quantity = 4,
                    ItemDescription = "Widget A",
                    MaybeHalfAPair = "N"
                }
            };

            this.result = this.Sut.GetContainerDescription(1);
        }

        [Test]
        public void ShouldSumQuantitiesForSameDescription()
        {
            this.result.Should().Be("9 Widget A");
        }
    }
}
