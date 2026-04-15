namespace Linn.Stores2.Integration.Tests.ConsignmentModuleTests
{
    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPackingListHtml : ContextBase
    {
        private string stringResponse;

        [SetUp]
        public void SetUp()
        {
            this.PackingListService
                .GetPackingListAsHtml(1)
                .Returns("<html><body>Packing List</body></html>");

            this.stringResponse = this.Client.GetStringAsync("stores2/consignments/1/packing-list/view").Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var htmlResponse = this.stringResponse;
            htmlResponse.Should().Be("<html><body>Packing List</body></html>");
        }
    }
}
