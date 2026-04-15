namespace Linn.Stores2.Domain.LinnApps.Consignments.Services;

using System.Collections.Generic;
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

        return await pdfService.ConvertHtmlToPdf(packingListHtml, false, footerHtml);
    }

    public async Task<string> GetPackingListsAsHtml(IEnumerable<int> consignmentNumbers)
    {
        var html = string.Empty;
        foreach (var consignmentNumber in consignmentNumbers)
        {
            var documentHtml = await this.GetPackingListAsHtml(consignmentNumber);
            html += $"<div style=\"page-break-after: always;\">{documentHtml}</div>";
        }

        return html;
    }

    public async Task<Stream> GetPackingListsAsPdf(IEnumerable<int> consignmentNumbers)
    {
        var html = await this.GetPackingListsAsHtml(consignmentNumbers);
        var footerHtml = stringFromFileService.GetString("Footer.html").Result;

        return await pdfService.ConvertHtmlToPdf(html, false, footerHtml);
    }
}
