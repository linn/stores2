namespace Linn.Stores2.Domain.LinnApps.Consignments.Services;

using System.IO;
using System.Threading.Tasks;

using Linn.Common.Pdf;
using Linn.Common.Persistence;
using Linn.Common.Rendering;

using Linn.Stores2.Domain.LinnApps.Consignments.Models;

public class PackingListService(
    IRepository<Consignment, int> consignmentRepository,
    IHtmlTemplateService<PackingListDocument> documentTemplateService,
    IPdfService pdfService,
    IStringFromFileService stringFromFileService) : IPackingListService
{
    public async Task<string> GetPackingListAsHtml(int consignmentNumber)
    {
        var consignment = await consignmentRepository.FindByIdAsync(consignmentNumber);

        var document = new PackingListDocument
        {
            Consignment = consignment,
            CarrierReference = $"{consignment.AddressId}/{consignment.SalesAccountId}"
        };

        return await documentTemplateService.GetHtml(document);
    }

    public async Task<Stream> GetPackingListAsPdf(int consignmentNumber)
    {
        var packingListHtml = await this.GetPackingListAsHtml(consignmentNumber);
        var footerHtml = stringFromFileService.GetString("Footer.html").Result;

        return await pdfService.ConvertHtmlToPdf(packingListHtml, true, footerHtml);
    }
}
