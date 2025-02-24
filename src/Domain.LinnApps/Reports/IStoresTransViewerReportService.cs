namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;

    public interface IStoresTransViewerReportService
    {
        ResultsModel StoresTransViewerReport(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            string functionCode);
    }
}
