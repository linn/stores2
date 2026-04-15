namespace Linn.Stores2.Domain.LinnApps.Consignments.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IPackingListService
    {
        Task<string> GetPackingListAsHtml(int consignmentNumber);

        Task<Stream> GetPackingListAsPdf(int consignmentNumber);

        Task<string> GetPackingListsAsHtml(IEnumerable<int> consignmentNumbers);

        Task<Stream> GetPackingListsAsPdf(IEnumerable<int> consignmentNumbers);
    }
}
