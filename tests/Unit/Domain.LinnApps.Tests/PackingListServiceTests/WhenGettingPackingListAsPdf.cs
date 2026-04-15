namespace Linn.Stores2.Domain.LinnApps.Tests.PackingListServiceTests
{
    using System.IO;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Domain.LinnApps.Consignments.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPackingListAsPdf : ContextBase
    {
        private Stream result;

        private MemoryStream pdfStream;

        [SetUp]
        public async Task SetUp()
        {
            var consignment = new Consignment { ConsignmentId = 1, AddressId = 100, SalesAccountId = 200 };

            this.ConsignmentRepository.FindByIdAsync(1).Returns(consignment);

            this.HtmlTemplateService
                .GetHtml(Arg.Any<PackingListDocument>())
                .Returns("<html>Packing List</html>");

            this.StringFromFileService.GetString("Footer.html").Returns("footer");

            this.pdfStream = new MemoryStream();
            this.PdfService
                .ConvertHtmlToPdf(Arg.Any<string>(), false, Arg.Any<string>())
                .Returns(this.pdfStream);

            this.result = await this.Sut.GetPackingListAsPdf(1);
        }

        [Test]
        public void ShouldReturnPdfStream()
        {
            this.result.Should().BeSameAs(this.pdfStream);
        }

        [Test]
        public void ShouldConvertHtmlToPdf()
        {
            this.PdfService.Received().ConvertHtmlToPdf(
                Arg.Is<string>(h => h.Contains("Packing List")),
                false,
                "footer");
        }
    }
}
