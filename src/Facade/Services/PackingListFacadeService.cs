namespace Linn.Stores2.Facade.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Linn.Common.Facade;
using Linn.Common.Service.Handlers;
using Linn.Stores2.Domain.LinnApps.Consignments.Services;

public class PackingListFacadeService : IPackingListFacadeService
{
    private readonly IPackingListService packingListService;

    public PackingListFacadeService(IPackingListService packingListService)
    {
        this.packingListService = packingListService;
    }

    public async Task<string> GetPackingListAsHtml(int consignmentNumber)
    {
        var result = await this.packingListService.GetPackingListAsHtml(consignmentNumber);

        return result;
    }

    public async Task<IResult<StreamResponse>> GetPackingListAsPdf(int consignmentNumber)
    {
        var result = await this.packingListService.GetPackingListAsPdf(consignmentNumber);

        return new SuccessResult<StreamResponse>(
            new StreamResponse
            {
                ContentType = "application/pdf", FileName = $"PackingList_{consignmentNumber}.pdf", Stream = result
            });
    }

    public async Task<string> GetPackingListsAsHtml(IEnumerable<int> consignmentNumbers)
    {
        return await this.packingListService.GetPackingListsAsHtml(consignmentNumbers);
    }

    public async Task<IResult<StreamResponse>> GetPackingListsAsPdf(IEnumerable<int> consignmentNumbers)
    {
        var result = await this.packingListService.GetPackingListsAsPdf(consignmentNumbers);

        return new SuccessResult<StreamResponse>(
            new StreamResponse
            {
                ContentType = "application/pdf",
                FileName = $"PackingLists_{DateTime.Now:yyyyMMdd}.pdf",
                Stream = result
            });
    }
}
