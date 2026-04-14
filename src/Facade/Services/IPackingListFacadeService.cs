namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service.Handlers;

    public interface IPackingListFacadeService
    {
        Task<string> GetPackingListAsHtml(int consignmentNumber);

        Task<IResult<StreamResponse>> GetPackingListAsPdf(int consignmentNumber);
    }
}
