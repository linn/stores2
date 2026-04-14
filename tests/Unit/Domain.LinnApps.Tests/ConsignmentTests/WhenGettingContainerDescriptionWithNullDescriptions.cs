namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingContainerDescriptionWithNullDescriptions : ContextBase
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
                    ItemDescription = "Valid Item",
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "I",
                    Quantity = 3,
                    ItemDescription = null,
                    MaybeHalfAPair = "N"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "S",
                    Quantity = 2,
                    ItemDescription = string.Empty,
                    MaybeHalfAPair = "N"
                }
            };

            this.result = this.Sut.GetContainerDescription(1);
        }

        [Test]
        public void ShouldIncludeValidDescription()
        {
            this.result.Should().Be("5 Valid Item");
        }

        [Test]
        public void ShouldNotIncludeNullOrEmptyDescriptions()
        {
            this.result.Should().NotContain("3 ");
            this.result.Should().NotContain("2 ");
        }
    }
}
