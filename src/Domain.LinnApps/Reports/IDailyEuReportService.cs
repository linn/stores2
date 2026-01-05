namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Reporting.Models;

    public interface IDailyEuReportService
    {
        Task<ResultsModel> GetDailyEuImportRsnReport(
            string startDate,
            string toDate);

        Task<ResultsModel> GetDailyEuDespatchReport(DateTime startDate, DateTime toDate);

        Task<ResultsModel> GetDailyEuDespatchRsnReport(DateTime startDate, DateTime toDate);
    }
}
