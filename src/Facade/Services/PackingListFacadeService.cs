namespace Linn.Stores2.Facade.Services;

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
}
