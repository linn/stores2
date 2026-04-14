namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingContainerDescriptionWithPairs : ContextBase
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
                    Quantity = 2.5m,
                    ItemDescription = "Speaker Pair",
                    MaybeHalfAPair = "Y"
                },
                new ConsignmentItem
                {
                    ContainerNumber = 1,
                    ItemType = "S",
                    Quantity = 1.5m,
                    ItemDescription = "Headphone Pair",
                    MaybeHalfAPair = "Y"
                }
            };

            this.result = this.Sut.GetContainerDescription(1);
        }

        [Test]
        public void ShouldDoubleQuantityForPairs()
        {
            this.result.Should().Contain("5 Speaker Pair");
            this.result.Should().Contain("3 Headphone Pair");
        }
    }
}
