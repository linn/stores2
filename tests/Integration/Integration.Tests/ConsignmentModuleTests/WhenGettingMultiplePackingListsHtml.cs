namespace Linn.Stores2.Integration.Tests.ConsignmentModuleTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingMultiplePackingListsHtml : ContextBase
    {
        private string stringResponse;

        [SetUp]
        public void SetUp()
        {
            this.PackingListService
                .GetPackingListsAsHtml(Arg.Any<IEnumerable<int>>())
                .Returns("<html><body>Multiple Packing Lists</body></html>");

            this.stringResponse = this.Client.GetStringAsync("stores2/consignments/multiple-packing-lists/view?consignmentNumber=1&consignmentNumber=2").Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var htmlResponse = this.stringResponse;
            htmlResponse.Should().Be("<html><body>Multiple Packing Lists</body></html>");
        }
    }
}
