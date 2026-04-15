namespace Linn.Stores2.Domain.LinnApps.Tests.PackingListServiceTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPackingListAsHtml : ContextBase
    {
        private string result;

        [SetUp]
        public async Task SetUp()
        {
            var consignment = new Consignment { ConsignmentId = 1, AddressId = 100, SalesAccountId = 200 };

            this.ConsignmentRepository.FindByIdAsync(1).Returns(consignment);

            this.HtmlTemplateService
                .GetHtml(Arg.Is<PackingListDocument>(d => d.Consignment.ConsignmentId == 1))
                .Returns("<html>Packing List</html>");

            this.result = await this.Sut.GetPackingListAsHtml(1);
        }

        [Test]
        public void ShouldReturnHtml()
        {
            this.result.Should().Be("<html>Packing List</html>");
        }

        [Test]
        public void ShouldCallHtmlService()
        {
            this.HtmlTemplateService.Received().GetHtml(
                Arg.Is<PackingListDocument>(d => d.CarrierReference == "100/200"));
        }
    }
}
