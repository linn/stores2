namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingContainerDescriptionWithNoItems : ContextBase
    {
        private string result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>();
            this.result = this.Sut.GetContainerDescription(1);
        }

        [Test]
        public void ShouldReturnEmptyString()
        {
            this.result.Should().BeEmpty();
        }
    }
}
