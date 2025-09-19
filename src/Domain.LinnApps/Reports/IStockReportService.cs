namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Threading.Tasks;
    using Linn.Common.Reporting.Models;

    public interface IStockReportService
    {
        Task<ResultsModel> GetStockInLabourHours(string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true);

        Task<decimal> GetStockInLabourHoursTotal(string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true);

        Task<ResultsModel> GetLabourHoursSummaryReport(
            DateTime fromDate,
            DateTime toDate,
            string accountingCompany = "LINN");
    }
}
