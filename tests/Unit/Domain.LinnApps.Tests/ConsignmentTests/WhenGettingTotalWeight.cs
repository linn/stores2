namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingTotalWeight : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { ContainerNumber = 1, ItemType = "C", Weight = 10m, Height = 10, Depth = 20, Width = 30 }
            };

            this.Sut.Pallets = new List<ConsignmentPallet>
            {
                new ConsignmentPallet { PalletNumber = 1, Weight = 5 }
            };
        }

        [Test]
        public void ShouldReturnSumOfBoxWeightsAndPalletWeights()
        {
            this.Sut.TotalWeight().Should().Be(15m);
        }
    }
}
