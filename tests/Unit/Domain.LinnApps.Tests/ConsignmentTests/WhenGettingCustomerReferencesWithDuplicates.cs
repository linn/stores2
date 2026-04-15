namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingCustomerReferencesWithDuplicates : ContextBase
    {
        private string result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { SalesOrder = new SalesOrder { CustomerOrderNumber = "REF001" } },
                new ConsignmentItem { SalesOrder = new SalesOrder { CustomerOrderNumber = "REF001" } },
                new ConsignmentItem { SalesOrder = new SalesOrder { CustomerOrderNumber = "REF002" } }
            };

            this.result = this.Sut.GetCustomerReferences();
        }

        [Test]
        public void ShouldDeduplicateReferences()
        {
            this.result.Should().Be("REF001, REF002");
        }
    }
}
