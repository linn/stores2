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

        Task<ResultsModel> GetDailyEuDispatchReport(DateTime startDate, DateTime toDate);

        Task<ResultsModel> GetDailyEuRsnDispatchReport(DateTime fromDate, DateTime toDate);
    }
}
