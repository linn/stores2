namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System.Threading.Tasks;

    using Linn.Common.Reporting.Models;

    public interface IGoodsInLogReportService
    {
        Task <ResultsModel> GoodsInLogReport(
            string fromDate,
            string toDate,
            int? createdBy,
            string articleNumber,
            decimal? quantity,
            int? orderNumber,
            int? reqNumber,
            string storagePlace);
    }
}
