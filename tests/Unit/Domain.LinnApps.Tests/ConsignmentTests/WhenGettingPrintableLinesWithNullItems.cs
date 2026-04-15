namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NUnit.Framework;

    public class WhenGettingPrintableLinesWithNullItems : ContextBase
    {
        private IList<ConsignmentPrintLine> result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = null;
            this.result = this.Sut.GetPrintableLines().ToList();
        }

        [Test]
        public void ShouldReturnEmptyList()
        {
            this.result.Should().BeEmpty();
        }
    }
}
