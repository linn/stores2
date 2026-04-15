namespace Linn.Stores2.Domain.LinnApps.Tests.PackingListServiceTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPackingListsAsHtml : ContextBase
    {
        private string result;

        [SetUp]
        public async Task SetUp()
        {
            var consignment1 = new Consignment { ConsignmentId = 1, AddressId = 100, SalesAccountId = 200 };
            var consignment2 = new Consignment { ConsignmentId = 2, AddressId = 101, SalesAccountId = 201 };

            this.ConsignmentRepository.FindByIdAsync(1).Returns(consignment1);
            this.ConsignmentRepository.FindByIdAsync(2).Returns(consignment2);

            this.HtmlTemplateService
                .GetHtml(Arg.Any<PackingListDocument>())
                .Returns("<html>Packing List</html>");

            this.result = await this.Sut.GetPackingListsAsHtml([1, 2]);
        }

        [Test]
        public void ShouldReturnHtmlForEachConsignment()
        {
            this.HtmlTemplateService.Received(2).GetHtml(Arg.Any<PackingListDocument>());
        }

        [Test]
        public void ShouldWrapEachDocumentInPageBreakDiv()
        {
            this.result.Should().Contain("page-break-after: always");
        }

        [Test]
        public void ShouldCombineAllHtml()
        {
            this.result.Should().Contain("<html>Packing List</html>");
        }
    }
}
