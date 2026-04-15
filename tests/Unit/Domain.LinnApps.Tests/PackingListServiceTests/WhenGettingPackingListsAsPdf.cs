namespace Linn.Stores2.Domain.LinnApps.Tests.PackingListServiceTests
{
    using System.IO;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPackingListsAsPdf : ContextBase
    {
        private Stream result;

        private MemoryStream pdfStream;

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

            this.StringFromFileService.GetString("Footer.html").Returns("footer");

            this.pdfStream = new MemoryStream();
            this.PdfService
                .ConvertHtmlToPdf(Arg.Any<string>(), false, Arg.Any<string>())
                .Returns(this.pdfStream);

            this.result = await this.Sut.GetPackingListsAsPdf([1, 2]);
        }

        [Test]
        public void ShouldReturnPdfStream()
        {
            this.result.Should().BeSameAs(this.pdfStream);
        }

        [Test]
        public void ShouldConvertCombinedHtmlToPdfOnce()
        {
            this.PdfService.Received(1).ConvertHtmlToPdf(Arg.Any<string>(), false, "footer");
        }
    }
}
