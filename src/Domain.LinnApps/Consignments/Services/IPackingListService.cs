namespace Linn.Stores2.Domain.LinnApps.Consignments.Services
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IPackingListService
    {
        Task<string> GetPackingListAsHtml(int consignmentNumber);

        Task<Stream> GetPackingListAsPdf(int consignmentNumber);
    }
}
