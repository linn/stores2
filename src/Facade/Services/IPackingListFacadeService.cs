namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Service.Handlers;

    public interface IPackingListFacadeService
    {
        Task<string> GetPackingListAsHtml(int consignmentNumber);

        Task<IResult<StreamResponse>> GetPackingListAsPdf(int consignmentNumber);

        Task<string> GetPackingListsAsHtml(IEnumerable<int> consignmentNumbers);

        Task<IResult<StreamResponse>> GetPackingListsAsPdf(IEnumerable<int> consignmentNumbers);
    }
}
