namespace Linn.Stores2.Domain.LinnApps.Tests.ConsignmentTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;

    using NUnit.Framework;

    public class WhenGettingCustomerReferencesWithNullSalesOrders : ContextBase
    {
        private string result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Items = new List<ConsignmentItem>
            {
                new ConsignmentItem { SalesOrder = new SalesOrder { CustomerOrderNumber = "REF001" } },
                new ConsignmentItem { SalesOrder = null },
                new ConsignmentItem { SalesOrder = new SalesOrder { CustomerOrderNumber = null } },
                new ConsignmentItem { SalesOrder = new SalesOrder { CustomerOrderNumber = string.Empty } }
            };

            this.result = this.Sut.GetCustomerReferences();
        }

        [Test]
        public void ShouldOnlyReturnValidReferences()
        {
            this.result.Should().Be("REF001");
        }
    }
}
